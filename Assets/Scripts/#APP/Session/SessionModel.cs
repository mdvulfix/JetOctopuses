using System;
using APP.Scene;
using APP.Signal;

namespace APP
{
    public abstract class SessionModel<TSession>: SceneObject, IConfigurable, IInitializable, ISubscriber
    where TSession : SceneObject, ISession
    {
        private SessionConfig m_Config;

        
        private ISceneController m_SceneController;
        private IStateController m_StateController;
        private bool m_Debug;

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public bool IsConfigured {get; private set; }
        public bool IsInitialized {get; private set; }

        public IState StateActive { get; private set; }

    
        // CONFIGURE //
        public virtual void Configure() =>
            Configure(config: null);

        public virtual void Configure(IConfig config) =>
            Configure(config: config, param: null);

        public virtual void Configure (IConfig config, params object[] param)
        {
            if(config != null)
            {
                m_Config = (SessionConfig)config;
            
            }          
               
            if(param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            m_SceneController = new SceneControllerDefault(new SceneControllerConfig());
            m_StateController = new StateControllerDefault(new StateControllerConfig());
            
            
            OnConfigured();
        }


    
        // INIT //
        public virtual void Init()
        {
            Subscribe();
            
            m_StateController.Init();
            m_SceneController.Init();
        }
    
        public virtual void Dispose()
        {
            m_SceneController.Dispose();
            m_StateController.Dispose();

            Unsubscribe();
        }

        // SUBSCRUBE //
        public virtual void Subscribe()
        {
            m_StateController.StateChanged += OnStateChanged;
        }

        public virtual void Unsubscribe()
        {
            m_StateController.StateChanged -= OnStateChanged;
        }

       
        //protected virtual void StateUpdate(IState state) { }
       
        
        protected void StateExecute<TState>() where TState: class, IState
        {
            //StateActive = m_StateController.Execute<TState>();
            OnStateChanged(StateActive);
        }

        // HELPER //
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

        // CALLBACK //
        private void OnConfigured()
        {
            Send($"Configuration successfully completed!");
            IsConfigured = true;
            Configured?.Invoke();
        }
        
        private void OnInitialized()
        {
            Send($"Initialization successfully completed!");
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDisposed()
        {
            Send($"Dispose process successfully completed!");
            IsInitialized = false;
            Disposed?.Invoke();
        }

        private void OnStateChanged(IState state)
        {
            Send($"State changed. {state} state activated...");
            //StateUpdate(StateActive);
        }

        private void OnSignalCalled(ISignal signal)
        {

            //signal
            
            
            //Send($"State changed. {state} state activated...");
            //StateActive = state;
            //StateUpdate(StateActive);
        }
        

        // UNITY //
        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();

        
    }

}