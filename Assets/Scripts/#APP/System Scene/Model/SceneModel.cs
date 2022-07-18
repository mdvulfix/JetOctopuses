using System;
using System.Threading.Tasks;
using APP.Screen;
using SERVICE.Handler;
using UnityEngine;

namespace APP.Scene
{
    public abstract class SceneModel<TScene>:
        IConfigurable, IInitializable, ICacheable, ISubscriber, IMessager
    where TScene : IScene
    {
        [SerializeField]
        private bool m_Debug = true;

        private SceneConfig m_Config;

        private ICacheHandler m_CacheHandler;

        private IScreenController m_ScreenController;

        private IScreen[] m_Screens;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsActivated { get; private set; }

        public string Label { get; private set; }
        public IScene Scene { get; private set; }
        public SceneIndex Index { get; private set; }
        
        public IScreen ScreenLoading { get; private set; }
        public IScreen ScreenStart { get; private set; }

        public ISceneObject SceneObject { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action Loaded;
        public event Action Activated;

        public event Action<IMessage> Message;

        public event Action<ICacheable> RecordToCahceRequired;
        public event Action<ICacheable> DeleteFromCahceRequired;

        // CONFIGURE //
        public virtual IMessage Configure(IConfig config = null, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);

            if (config != null)
            {
                m_Config = (SceneConfig) config;

                Label = m_Config.Label;
                Scene = m_Config.Scene;
                Index = SceneIndex<TScene>.SetIndex(m_Config.SceneIndex);

                m_Screens = m_Config.Screens;
                ScreenLoading = m_Config.ScreenLoading;
                ScreenStart = m_Config.ScreenStart;

            }

            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is object)
                        Send("Param is not used", LogFormat.Worning);
                }
            }

            m_CacheHandler = new CacheHandler<IScene>();
            Send(m_CacheHandler.Configure(new CacheHandlerConfig(Scene)), SendFormat.Sender);

            m_ScreenController = new ScreenControllerDefault();
            Send(m_ScreenController.Configure(new ScreenControllerConfig(m_Config.Screens)), SendFormat.Sender);

            IsConfigured = true;
            Configured?.Invoke();

            return Send("Configuration completed!");
        }

        // INIT //
        public virtual IMessage Init()
        {
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);

            Subscribe();

            Send(m_CacheHandler.Init(), SendFormat.Sender);
            Send(m_ScreenController.Init(), SendFormat.Sender);

            RecordToCache();

            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {
            DeleteFromCache();

            Send(m_ScreenController.Dispose(), SendFormat.Sender);
            Send(m_CacheHandler.Dispose(), SendFormat.Sender);

            Unsubscribe();

            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
        }

        public virtual void Subscribe()
        {
            m_CacheHandler.Message += OnMessage;

        }

        public virtual void Unsubscribe()
        {
            m_CacheHandler.Message -= OnMessage;
        }

        public async Task<ITaskResult> Load()
        {
            if (IsLoaded == true)
                return new TaskResult(true, Send("The instance was already loaded. The current loading has been aborted!", LogFormat.Worning));

            var uSceneLoadingTaskResult = await SceneHandler.USceneLoad(Index);
            if(uSceneLoadingTaskResult.Status == false)
                return new TaskResult(false, uSceneLoadingTaskResult.Message);
            
            
            var uSceneActivateTaskResult = await SceneHandler.USceneActivate(Index);
            if(uSceneActivateTaskResult.Status == false)
                return new TaskResult(false, uSceneActivateTaskResult.Message);
            
            
            // Loading scene objects  ...
            await TaskHandler.Run(() => AwaitSceneLoading(), "Waiting for screen loading...");

            
            // Loading screens...
            foreach (var screen in m_Screens)
            {
                var screenLoadTaskResult = await m_ScreenController.ScreenLoad(screen);
                if (screenLoadTaskResult.Status == false)
                    return new TaskResult(false, screenLoadTaskResult.Message);
            }

            IsLoaded = true;
            Loaded?.Invoke();
            return new TaskResult(true, Send($"All screens were loaded. {ScreenLoading} was activated."));
        }

        public async Task<ITaskResult> Activate<TScreen>(bool screenActivate = true, bool screenAnimate = true)
        where TScreen : IScreen
        {
            foreach (var screen in m_Screens)
            {
                if (screen.GetType() == typeof(TScreen))
                {
                    // Activate loading screen...
                    var screenActivateTaskResult = await m_ScreenController.ScreenActivate(ScreenLoading, screenActivate, screenAnimate);
                    if (screenActivateTaskResult.Status == false)
                        return new TaskResult(false, screenActivateTaskResult.Message);
                }
            }

            return new TaskResult(false, Send($"{typeof(TScreen)} was not found. Activation has been failed!", LogFormat.Worning));
        }

        public async Task<ITaskResult> Activate(IScreen screen, bool screenActivate = true, bool screenAnimate = true)
        {
            if (IsLoaded == false)
            {
                var sceneLoadTaskResult = await Load();
                if (sceneLoadTaskResult.Status == false)
                    return new TaskResult(false, sceneLoadTaskResult.Message);
            }

            if (IsActivated == true)
                return new TaskResult(true, Send("The scene was already activated. The current loading has been activated!", LogFormat.Worning));
            
            
            var uSceneActivateTaskResult = await SceneHandler.USceneActivate(Index);
            if(uSceneActivateTaskResult.Status == false)
                return new TaskResult(false, uSceneActivateTaskResult.Message);
            
            // Activate  UScene...
            var sceneActivate = true;
            await TaskHandler.Run(() => AwaitSceneActivation(sceneActivate), "Waiting for screen deactivation...");

            // Activate loading screen...
            var screenLoadingActivate = true;
            var screenLoadingAnimate = true;
            var screenLoadingActivateTaskResult = await m_ScreenController.ScreenActivate(ScreenLoading, screenLoadingActivate, screenLoadingAnimate);
            
            if (screenLoadingActivateTaskResult.Status == false)
                return new TaskResult(false, screenLoadingActivateTaskResult.Message);

            await Task.Delay(100);

            // Activate target screen...
            var screenTargetActivateTaskResult = await m_ScreenController.ScreenActivate(screen, screenActivate, screenAnimate);
            if (screenTargetActivateTaskResult.Status == false)
                return new TaskResult(false, screenTargetActivateTaskResult.Message);

            return new TaskResult(true, Send($"{screen.GetType().Name} was activated."));
        }

        // AWAIT //
        private bool AwaitSceneLoading()
        {
        
            if (SceneObject == null)
                SceneObject = SetComponent<SceneObject>(Label, Scene.SceneObject);

            return true;
        }

        private bool AwaitSceneActivation(bool activate)
        {
            if (SceneObject != null)
                SceneObject.gameObject.SetActive(activate);
            else
                throw new Exception();

            return true;
        }

        protected TComponent SetComponent<TComponent>(string name = null, ISceneObject parent = null)
        where TComponent : Component, ISceneObject
        {
            if (name == null)
            {
                return SceneHandler.SetComponent<TComponent>(SceneObject.gameObject);
            }
            else
            {
                var obj = SceneHandler.SetGameObject(name, parent != null ? parent.gameObject : null);
                return SceneHandler.SetComponent<TComponent>(obj);
            }
        }

        // MESSAGE //
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message, SendFormat sendFrom = SendFormat.Self)
        {
            Message?.Invoke(message);

            switch (sendFrom)
            {
                case SendFormat.Sender:
                    return Messager.Send(m_Debug, this, $"message from: {message.Text}", message.LogFormat);

                default:
                    return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
            }
        }

        // CACHE //
        private void RecordToCache() =>
            RecordToCahceRequired?.Invoke(Scene);

        private void DeleteFromCache() =>
            DeleteFromCahceRequired?.Invoke(Scene);

        // CALLBACK //
        private void OnMessage(IMessage message) =>
            Send(message);

    }

    public struct SceneConfig : IConfig
    {
        public string Label { get; }
        public IScene Scene { get; private set; }
        public SceneIndex SceneIndex { get; }
        public IScreen[] Screens { get; private set; }
        public IScreen ScreenLoading { get; }
        public IScreen ScreenStart { get; }

        public SceneConfig(
            IScene scene,
            SceneIndex index,
            IScreen[] screens,
            IScreen screenLoading,
            IScreen screenStart,
            string label = "Scene: ")
        {
            Label = label;
            Scene = scene;
            SceneIndex = index;
            Screens = screens;
            ScreenLoading = screenLoading;
            ScreenStart = screenStart;
        }

    }

    public struct SceneActionInfo : IActionInfo
    {
        public IScene Scene { get; private set; }

        public SceneActionInfo(IScene scene)
        {
            Scene = scene;
        }

    }

}

namespace APP
{
    public interface IScene : IConfigurable, IInitializable, ICacheable
    {

        bool IsLoaded { get; }
        bool IsActivated { get; }

        SceneIndex Index { get; }

        ISceneObject SceneObject { get; }

        IScreen ScreenLoading { get; }
        IScreen ScreenStart { get; }

        Task<ITaskResult> Load();
        Task<ITaskResult> Activate(IScreen screen, bool screenActivate = true, bool screenAnimate = true);
        Task<ITaskResult> Activate<TScreen>(bool screenActivate = true, bool screenAnimate = true)
        where TScreen : IScreen;

    }

}

/*
        public event Action<IEventArgs> PlayButtonClicked;
        public event Action<IEventArgs> OptionsButtonClicked;
        public event Action<IEventArgs> ExitButtonClicked;

        protected override void OnAwake()
        {

        }

        protected override void OnEnable()
        {
            m_Play.onClick.AddListener(() => Play());
            m_Options.onClick.AddListener(() => Options());
            m_Exit.onClick.AddListener(() => Exit());
            
            Add(this);

        }

        protected override void OnDisable()
        {
            m_Play.onClick.RemoveListener(() => Play());
            m_Options.onClick.RemoveListener(() => Options());
            m_Exit.onClick.RemoveListener(() => Exit());

            Remove(this);
        }
    
    
        private void Play(IEventArgs args = null)
        {
            PlayButtonClicked?.Invoke(args);
        }

        private void Options(IEventArgs args = null)
        {
            OptionsButtonClicked?.Invoke(args);
        }

        private void Exit(IEventArgs args = null)
        {
            ExitButtonClicked?.Invoke(args);
        }

        */