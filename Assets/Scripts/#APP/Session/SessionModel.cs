using System;
using APP.Scene;
using APP.Signal;

namespace APP
{
    public abstract class SessionModel<TSession>: SceneObject, IConfigurable, IInitializable, ISubscriber
    where TSession : SceneObject, ISession
    {
        private bool m_Debug = true;

        private SessionConfig m_Config;
        private ISession m_Session;
        
        private ISceneController m_SceneController;
        private IStateController m_StateController;
    
        public bool IsConfigured {get; private set; }
        public bool IsInitialized {get; private set; }
        
        public string Label {get; private set;}
        public ISession Session {get; private set;}
        public SceneObject SceneObject {get; private set;}
        
        public IScene[] Scenes {get; private set; }
        public IState[] States {get; private set; }
        
        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;



        public IState StateActive { get; private set; }

        // CONFIGURE //
        public void Configure (IConfig config, params object[] param)
        {
            if(config != null)
            {
                m_Config = (SessionConfig)config;

                Label = m_Config.Label;
                Session = m_Config.Session;
                
                Scenes = m_Config.Scenes;
                States = m_Config.States;
            
            }          
               
            if(param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            
            foreach (var scene in Scenes)
                scene.Configure();

            foreach (var state in States)
                state.Configure();
            
            m_SceneController = new SceneControllerDefault(new SceneControllerConfig());
            m_StateController = new StateControllerDefault(new StateControllerConfig());
            
            
            OnConfigured();
        }

        // INIT //
        public virtual void Init()
        {
            Subscribe();
            
            
            foreach (var scene in Scenes)
                scene.Init();

            foreach (var state in States)
                state.Init();
            
            
            m_StateController.Init();
            m_SceneController.Init();

            OnInitialized();
        }
    
        public virtual void Dispose()
        {
            
            foreach (var scene in Scenes)
                scene.Dispose();

            foreach (var state in States)
                state.Dispose();
            
            
            m_SceneController.Dispose();
            m_StateController.Dispose();

            Unsubscribe();
            OnDisposed();
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
            Messager.Send(m_Debug, this, text, worning);

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
        
    }

}