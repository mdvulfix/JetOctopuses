using System;
using System.Threading.Tasks;
using UnityEngine;

using SERVICE.Handler;

namespace APP.Screen
{

    [Serializable]
    public abstract class ScreenModel<TScreen>: IConfigurable, ICacheable, IMessager
    where TScreen : IScreen
    {

        [SerializeField] private bool m_Debug = false;

        private ScreenConfig m_Config;
        private IScreenController m_ScreenController;
        private Animator m_Animator;

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action RecordRequired;
        public event Action DeleteRequired;

        public event Action<IMessage> Message;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }
        public bool IsLoaded { get; private set; }

        public string Label { get; private set; }
        public IScene Scene { get; private set; }
        public IScreen Screen { get; private set; }
        public IButton[] Buttons { get; private set; }

        public ISceneObject SceneObject { get; private set; }

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
                        m_Config = (ScreenConfig) obj;

                        Label = m_Config.Label;
                        Scene = m_Config.Scene;
                        Screen = m_Config.Screen;
                        Buttons = m_Config.Buttons;

                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            

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

            IsInitialized = true;
            Initialized?.Invoke();
            
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {
            Send("Start disposing...");

            Unsubscribe();

            IsLoaded = false;
            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }

        
        // SUBSCRUBE //
        public virtual void Subscribe() { }
        public virtual void Unsubscribe() { }

        public async Task<ITaskResult> Load()
        {
            if (IsLoaded == true)
                return new TaskResult(true, Send("The instance was already loaded. The current loading has been aborted!", LogFormat.Worning));
            

            await TaskHandler.Run(() => AwaitScreenLoading(), "Waiting for screen loading...");
            
            IsLoaded = true;
            return new TaskResult(true, Send("The instance was loaded."));
            
        }

        public async Task<ITaskResult> Activate(bool activate = true, bool animate = true)
        {
            if(activate == true)
            {
                await TaskHandler.Run(() => AwaitScreenActivation(true), "Waiting for screen activation...");
                if(animate == true)
                    await Animate(true);
                else
                    await Animate(false);
            }
            else
            {
                await Animate(false);
                await TaskHandler.Run(() => AwaitScreenActivation(false), "Waiting for screen deactivation...");
            }

            return new TaskResult(true, Send("The instance was activated.")); 
        }

        public async Task<ITaskResult> Animate(bool animate = true)
        {
            //m_Animator.PlayAnimation(animate);
            await TaskHandler.Run(() => AwaitScreenAnimation(), "Waiting for screen animation...");
            return new TaskResult(true, Send("The instance was animated."));
        }

        // AWAIT //
        private bool AwaitScreenLoading()
        {
            if(SceneObject == null)
                SceneObject = SetComponent<SceneObject>(Label, Scene.SceneObject);

            return true;
        }

        private bool AwaitScreenActivation(bool activate)
        {
            SceneObject.gameObject.SetActive(activate); 
            return true;              
        }

        private bool AwaitScreenAnimation()
        {
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

        public IMessage Send(IMessage message)
        {
            Message?.Invoke(message);
            return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
        }
        
        // CALLBACK //
        public void OnMessage(IMessage message) =>
            Send($"{message.Sender}: {message.Text}", message.LogFormat);

    }

    public struct ScreenConfig : IConfig
    {
        public IScreen Screen { get; private set; }
        public IScene Scene { get; private set; }
        public IButton[] Buttons { get; private set; }
        public string Label { get; private set; }

        public ScreenConfig(IScreen screen, IScene scene, IButton[] buttons, string label = "Screen: ...")
        {
            Label = label;
            Scene = scene;
            Screen = screen;
            Buttons = buttons;
        }
    }
}

namespace APP
{
    public interface IScreen : IConfigurable, ICacheable, IMessager
    {
        bool IsLoaded { get; }
        
        ISceneObject SceneObject { get; }
        
        Task<ITaskResult> Load();
        Task<ITaskResult> Activate(bool activate = true, bool animate = true);
        Task<ITaskResult> Animate(bool animate = true);
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