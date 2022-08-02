using System;
using System.Threading.Tasks;
using APP.Screen;
using SERVICE.Handler;
using UnityEngine;

namespace APP.Scene
{
    public abstract class SceneModel<TScene>: IConfigurable, ICacheable, ISubscriber, IMessager
    where TScene : IScene
    {
        [SerializeField]
        private bool m_Debug = true;

        private SceneConfig m_Config;

        private IScreen[] m_Screens;
        private IScreen m_ScreenLoading;
        private IScreen m_ScreenDefault;

        private ICacheHandler m_CacheHandler;
        private IScreenController m_ScreenController;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsActivated { get; private set; }

        public string Label { get; private set; }
        public IScene Scene { get; private set; }
        public SceneIndex Index { get; private set; }
        


        public ISceneObject SceneObject { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action Loaded;
        public event Action Activated;

        public event Action RecordRequired;
        public event Action DeleteRequired;

        public event Action<IMessage> Message;



        // CONFIGURE //
        public virtual void Configure(params object[] param)
        {
            Send("Start configuration...");
                        
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Worning);
                return;
            }
                
            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (SceneConfig) obj;

                        Label = m_Config.Label;
                        Scene = m_Config.Scene;
                        Index = SceneIndex<TScene>.SetIndex(m_Config.SceneIndex);

                        m_Screens = m_Config.Screens;
                        m_ScreenLoading = m_Config.ScreenLoading;
                        m_ScreenDefault = m_Config.ScreenDefault;

                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            

            m_CacheHandler = new CacheHandler<IScene>();
            m_ScreenController = new ScreenControllerDefault();
            
            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }

        public virtual void Init()
        {
            
            Send("Start initialization...");
            
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Worning);
                return;
            }
                
            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Worning);
                return;
            }
                
            Subscribe();
            
            m_CacheHandler.Configure(new CacheHandlerConfig(this));
            m_CacheHandler.Init();
            RecordToCache();
            
            
            m_ScreenController.Configure(new ScreenControllerConfig(m_Screens, m_ScreenLoading, m_ScreenDefault));
            m_ScreenController.Init();


                

            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {

            Send("Start disposing...");
            
            foreach (var screen in m_Screens)
                screen.Dispose();
            
            m_ScreenController.Dispose();
            m_CacheHandler.Dispose();

            DeleteFromCache();
            Unsubscribe();

            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }

        public virtual void Subscribe()
        {
            m_CacheHandler.Message += OnMessage;
            m_ScreenController.Message += OnMessage;
            
            foreach (var screen in m_Screens)
                screen.Message += OnMessage;

        }

        public virtual void Unsubscribe()
        {
            m_CacheHandler.Message -= OnMessage;
            m_ScreenController.Message -= OnMessage;
            
            foreach (var screen in m_Screens)
                screen.Message += OnMessage;
        }

        // SCENE //
        public async Task<ITaskResult> Load()
        {
            if (IsLoaded == true)
                return new TaskResult(true, Send("The instance was already loaded. The current loading has been aborted!", LogFormat.Worning));

            var uSceneLoadingTaskResult = await USceneHandler.USceneLoad(Index);
            if(uSceneLoadingTaskResult.Status == false)
                return new TaskResult(false, uSceneLoadingTaskResult.Message);
            
            
            var uSceneActivateTaskResult = await USceneHandler.USceneActivate(Index);
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
            return new TaskResult(true, Send("The instance was loaded."));
        }

        public async Task<ITaskResult> Activate(bool animate = true)
        {
            if (IsActivated == true)
                return new TaskResult(true, Send("The scene was already activated. The current activation has been aborted!", LogFormat.Worning));
            
            var uSceneActivateTaskResult = await USceneHandler.USceneActivate(Index);
            if(uSceneActivateTaskResult.Status == false)
                return new TaskResult(false, uSceneActivateTaskResult.Message);
            
            // Activate  UScene...
            var sceneActivate = true;
            await TaskHandler.Run(() => AwaitSceneActivation(sceneActivate), "Waiting for screen activation...");

            var screenLoadTaskResult = await m_ScreenController.ScreenActivate(m_ScreenDefault, animate);
            if(screenLoadTaskResult.Status == false)
                return new TaskResult(false, screenLoadTaskResult.Message);

            IsActivated = true;
            Activated?.Invoke();
            return new TaskResult(true, Send("The instance was activated."));
        }

        public async Task<ITaskResult> Deactivate()
        {
            if (IsActivated != true)
                return new TaskResult(true, Send("The scene was already deactivated. The current deactivation has been aborted!", LogFormat.Worning));
            
            foreach (var screen in m_Screens)
            {
                var screenDeactivateTaskResult = await m_ScreenController.ScreenDeactivate(screen);
                if (screenDeactivateTaskResult.Status == false)
                    return new TaskResult(false, screenDeactivateTaskResult.Message);
            }
            
            // Activate  UScene...
            var sceneActivate = false;
            await TaskHandler.Run(() => AwaitSceneActivation(sceneActivate), "Waiting for screen deactivation...");

            IsActivated = false;
            return new TaskResult(true, Send("The instance was deactivated."));
        }

        public async Task<ITaskResult> Unload()
        {
            if (IsLoaded != true)
                return new TaskResult(true, Send("The instance was already unloaded. The current unloading has been aborted!", LogFormat.Worning));


            // Loading screens...
            foreach (var screen in m_Screens)
            {
                var screenLoadTaskResult = await m_ScreenController.ScreenUnload(screen);
                if (screenLoadTaskResult.Status == false)
                    return new TaskResult(false, screenLoadTaskResult.Message);
            }

            
            // Loading scene objects  ...
            await TaskHandler.Run(() => AwaitSceneUnloading(), "Waiting for scene unloading...");

            var sceneCoreIndex = SceneIndex<SceneCore>.Index;
            var uSceneActivateTaskResult = await USceneHandler.USceneActivate(sceneCoreIndex);
            if(uSceneActivateTaskResult.Status == false)
                return new TaskResult(false, uSceneActivateTaskResult.Message);

            
            var uSceneLoadingTaskResult = await USceneHandler.USceneLoad(Index);
            if(uSceneLoadingTaskResult.Status == false)
                return new TaskResult(false, uSceneLoadingTaskResult.Message);

            IsLoaded = true;
            Loaded?.Invoke();
            return new TaskResult(true, Send("The instance was loaded."));
        }

        // SCREEN //
        public async Task<ITaskResult> ScreenLoad(IScreen screen) =>
            await m_ScreenController.ScreenLoad(screen);

        public async Task<ITaskResult> ScreenActivate(IScreen screen, bool animate = true) =>
            await m_ScreenController.ScreenActivate(screen, animate);

        public async Task<ITaskResult> ScreenDeactivate(IScreen screen) =>
            await m_ScreenController.ScreenDeactivate(screen);

        public async Task<ITaskResult> ScreenUnload(IScreen screen) => 
            await m_ScreenController.ScreenUnload(screen);

        // AWAIT //
        private bool AwaitSceneLoading()
        {
            if(SceneObject != null)
                return true;

            var obj = GameObjectHandler.CreateGameObject(Label);
            SceneObject = GameObjectHandler.SetComponent<SceneObject>(obj);
   
            return true;
        }

        private bool AwaitSceneUnloading()
        {
            if(SceneObject == null)
                return true;
            
            var obj = SceneObject.gameObject;
            GameObjectHandler.DestroyGameObject(obj);
            return true;
        }

        private bool AwaitSceneActivation(bool activate)
        {
            if (SceneObject == null)
                return false;

            
            var obj = SceneObject.gameObject;
            obj.SetActive(activate); 
            return true;  
        }


        // MESSAGE //
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message)
        {
            Message?.Invoke(message);
            return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
        }
        
        public void OnMessage(IMessage message) =>
            Send($"{message.Sender.GetName()}: {message.Text}", message.LogFormat);

        // CACHE //
        private void RecordToCache() =>
            RecordRequired?.Invoke();

        private void DeleteFromCache() =>
            DeleteRequired?.Invoke();



    }

    public struct SceneConfig : IConfig
    {
        public string Label { get; }
        public IScene Scene { get; private set; }
        public SceneIndex SceneIndex { get; }
        public IScreen[] Screens { get; private set; }
        public IScreen ScreenLoading { get; }
        public IScreen ScreenDefault { get; }

        public SceneConfig(
            IScene scene,
            SceneIndex index,
            IScreen[] screens,
            IScreen screenLoading,
            IScreen screenDefault,
            string label = "Scene: ")
        {
            Label = label;
            Scene = scene;
            SceneIndex = index;
            Screens = screens;
            ScreenLoading = screenLoading;
            ScreenDefault = screenDefault;
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
    public interface IScene : IConfigurable, ICacheable
    {

        bool IsLoaded { get; }
        bool IsActivated { get; }

        SceneIndex Index { get; }

        ISceneObject SceneObject { get; }

        Task<ITaskResult> Load();
        Task<ITaskResult> Activate(bool animate = true);

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