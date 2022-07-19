using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APP.Scene;

namespace APP
{
    public abstract class SessionModel<TSession>: SceneObject, IConfigurable, ISubscriber, IMessager
    where TSession : SceneObject, ISession
    {
        private bool m_Debug = true;

        private SessionConfig m_Config;
        private ISession m_Session;

        private IScene m_SceneLoading;
        private IScene m_SceneStart;
        private ISceneController m_SceneController;
        
        private IStateController m_StateController;
        private IState[] m_States;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public string Label { get; private set; }
        public ISession Session { get; private set; }
        public ISceneObject SceneObject { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<IMessage> Message;

        public IState StateActive { get; private set; }

        public IScene SceneActive { get; private set; }
        
            
        // CONFIGURE //
        public virtual void Configure(params object[] param)
        {
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
                        m_Config = (SessionConfig) obj;

                        Label = m_Config.Label;
                        Session = m_Config.Session;
                        m_States = m_Config.States;

                        SceneObject = this;
                        
                        m_SceneLoading = m_Config.SceneLoading;
                        m_SceneStart = m_Config.SceneStart;

                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            
            m_SceneController = new SceneControllerDefault();


            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }

        public virtual void Init()
        {
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

            m_SceneController.Configure(new SceneControllerConfig());
            m_SceneController.Init();

            m_StateController.Configure(new StateControllerConfig());
            m_StateController.Init();
            
            foreach (var state in m_States)
                state.Configure();

            Send("All states configured!");

            foreach (var state in m_States)
                state.Init();

            Send("All states initialized!");

            //Execute<StateLoad>();


            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {
            //Execute<StateUnload>();
            
            foreach (var state in m_States)
                state.Dispose();

            Send("All states disposed!");
            
            m_SceneController.Dispose();
            m_StateController.Dispose();

            Unsubscribe();

            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }

        // SUBSCRUBE //
        public virtual void Subscribe()
        {
            
            Initialized += SceneActivate;
            //m_StateController.StateChanged += OnStateChanged;
        }

        public virtual void Unsubscribe()
        {
            //m_StateController.StateChanged -= OnStateChanged;
            Disposed -= SceneActivate;
        }

        //protected virtual void StateUpdate(IState state) { }

        public async Task SceneActivate(IScene scene, IScreen screen, bool screenActivate = true, bool screenAnimate = true) =>
            await m_SceneController.SceneActivate(scene, screen, screenActivate, screenAnimate);




        protected void StateExecute<TState>() where TState : class, IState
        {
            //StateActive = m_StateController.Execute<TState>();
            OnStateChanged(StateActive);
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

        private void OnStateChanged(IState state)
        {
            Send(new Message(this, $"State changed. {state} state activated..."));
            //StateUpdate(StateActive);
        }

        private void OnSignalCalled(ISignal signal)
        {

            //signal

            //Send($"State changed. {state} state activated...");
            //StateActive = state;
            //StateUpdate(StateActive);
        }

        private async void SceneActivate()
        {
            await SceneActivate(m_SceneLoading, m_SceneLoading.ScreenStart);
            await SceneActivate(m_SceneStart, m_SceneStart.ScreenStart);
        }


    // UNITY //
    private void Awake() =>
        Configure();

    private void OnEnable() =>
        Init();

    private void OnDisable() =>
        Dispose();

    }

     /*
        {
            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
            {
                Send($"{typeof(TScene).Name} is already active!");
                m_SceneActive.Activate<>
                return;
            }
            
            var sceneIndex = SceneIndex<TScene>.Index;
            await SceneHandler.Activate(sceneIndex);

            await TaskHandler.Run(() => AwaitSceneActivation<TScene>(sceneIndex, out scene), "Waiting for scene activation...");

            if (scene == null)
            {
                Send($"{scene.GetType().Name} not found!", LogFormat.Worning);
                return;
            }

            m_SceneActive = scene;
            m_SceneActive.Activate<ScreenLoading>();

        }

        */

}

namespace APP
{
    public interface ISession: IConfigurable
    {
        ISceneObject SceneObject { get; }

        IState StateActive { get; }
    }


}