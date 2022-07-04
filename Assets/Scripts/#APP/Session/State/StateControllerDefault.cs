using System;

namespace APP
{
    public class StateControllerDefault : Controller, IStateController
    {
        
        private StateControllerConfig m_Config;
        
        private Register<IState> m_Register;
        private IState m_StateActive;
        
        
        public StateControllerDefault() { }

        public StateControllerDefault(IConfig config) =>
            Configure(config);

        public bool IsConfigured {get; private set; }

        
        
        public event Action<IState> StateChanged;

        public void Configure(IConfig config)
        {

            m_Config = (StateControllerConfig)config;

            m_Register = new Register<IState>();
            
            foreach (var state in m_Config.States)
            {
                m_Register.Set(state);
            }

            IsConfigured = true;

        }

        public override void Init()
        {
            if(IsConfigured == false)
            {
                Send("Instance was not configured. Initialization was failed!", LogFormat.Worning);
                return;
            }
            
            IsInitialized = true;
        }
        
        public override void Dispose()
        {
            
            IsInitialized = false;
        }

        public TState Execute<TState>() where TState : class, IState
        {
            m_StateActive.Exit();
            
            if(m_Register.Get<TState>(out var state))
            {
                m_StateActive = state;
                m_StateActive.Enter();
                m_StateActive.Run();
                return state;
            }
            
            Send($"{typeof(TState).Name} was not registered!", LogFormat.Worning);
            return null;
        }


    }


    public class StateControllerConfig: Config
    {
        public IState[] States { get; private set; }

        public StateControllerConfig(Instance info, IState[] states): base(info)
        {
            States = states;
        }
    }



    public interface IStateController: IController, IConfigurable
    {
        event Action<IState> StateChanged;

        TState Execute<TState>() where TState: class, IState;
    }

}