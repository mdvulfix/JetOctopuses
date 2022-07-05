using System;
using APP.Scene;
using APP.Signal;

namespace APP
{
    public abstract class SessionModel<TSession>: IConfigurable, ICacheable
    where TSession : SceneObject, ISession
    {
        private SessionConfig m_Config;

        
        private ISceneController m_SceneController;
        private IStateController m_StateController;


    
        public bool IsConfigured {get; private set; }

        public IState StateActive { get; private set; }

        public virtual void Configure(IConfig config)
        {
            
            m_Config = (SessionConfig)config;
            
            m_SceneController = new SceneControllerDefault();
            var sceneControllerConfig = new SceneControllerConfig(m_SceneController, m_Config.Scenes);
            m_SceneController.Configure(sceneControllerConfig);

            m_StateController = new StateControllerDefault();
            var stateControllerInstance = new Instance(m_StateController);
            var stateControllerConfig = new StateControllerConfig(stateControllerInstance, m_Config.States);
            m_StateController.Configure(stateControllerConfig);

            base.Configure(config);
        }
    
    
        // INIT //
        public override void Init()
        {
            base.Init();
            m_StateController.Init();
            m_SceneController.Init();
        }
    
    
        public override void Dispose()
        {
            m_SceneController.Dispose();
            m_StateController.Dispose();
            base.Dispose();
        }


        // SUBSCRUBE //
        protected override void Subscrube()
        {
            base.Subscrube();
            m_StateController.StateChanged += OnStateChanged;
        }

        protected override void Unsubscrube()
        {
            m_StateController.StateChanged -= OnStateChanged;
            base.Unsubscrube();
        }

       
        //protected virtual void StateUpdate(IState state) { }
       
        
        
        private void OnSignalCalled(ISignal signal)
        {

            //signal
            
            
            //Send($"State changed. {state} state activated...");
            //StateActive = state;
            //StateUpdate(StateActive);
        }
        
        
        
        private void OnStateChanged(IState state)
        {
            Send($"State changed. {state} state activated...");
            //StateUpdate(StateActive);
        }
        
        
        
        protected void StateExecute<TState>() where TState: class, IState
        {
            StateActive = m_StateController.Execute<TState>();
            OnStateChanged(StateActive);
        }



        
    }

}