using System;
using System.Threading.Tasks;
using APP.Scene;
using APP.Signal;

namespace APP
{
    public abstract class SessionModel<TSession>:
        SceneObject, IConfigurable, IInitializable, ISubscriber, IMessager
    where TSession : SceneObject, ISession
    {
        private bool m_Debug = true;

        private SessionConfig m_Config;
        private ISession m_Session;

        
        private IScene m_SceneLoading;
        private IScene m_SceneStart;
        private ISceneController m_SceneController;
        
        private IStateController m_StateController;

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
        public virtual IMessage Configure(IConfig config, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);

            
            if (config != null)
            {
                m_Config = (SessionConfig) config;

                Label = m_Config.Label;
                Session = m_Config.Session;

                SceneObject = this;
                
                m_SceneLoading = m_Config.SceneLoading;
                m_SceneStart = m_Config.SceneStart;

                
                m_SceneController = new SceneControllerDefault();
                Send(m_SceneController.Configure(new SceneControllerConfig(m_Config.Scenes)), SendFormat.Sender);


                //m_StateController = new StateControllerDefault();
                //Send(m_StateController.Configure(new StateControllerConfig(m_Config.States)), SendFormat.Sender);
            }

            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is object)
                        Send("Param is not used", LogFormat.Worning);
                }
            }

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

            //Send(m_StateController.Init(), SendFormat.Sender);
            Send(m_SceneController.Init(), SendFormat.Sender);

        
            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {

            Send(m_SceneController.Dispose(), SendFormat.Sender);
            //Send(m_StateController.Dispose(), SendFormat.Sender);

            Unsubscribe();

            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
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
        // CALLBACK //
        private void OnMessage(IMessage message) =>
            Send(message);

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

    }


    public interface ISession: IConfigurable, IInitializable
    {
        
        ISceneObject SceneObject { get; }

        IState StateActive { get; }
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