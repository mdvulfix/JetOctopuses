using System;
using System.Collections.Generic;

namespace APP
{
    public class StateControllerDefault : Controller, IStateController
    {
        private StateControllerConfig m_Config;

        private IState m_StateActive;
        private IState m_StateLoad;
        private IState m_StateUnload;

        public StateControllerDefault() { }
        public StateControllerDefault(IConfig config) => Configure(config);

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }
        
        public IState[] States { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<IState> StateChanged;


        // CONFIGURE //
        public virtual IMessage Configure (IConfig config, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);
            
            if(config != null)
            {
                m_Config = (StateControllerConfig) config;

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
                
            foreach (var state in States)
                if(state.IsConfigured == false)
                    state.Configure();

            Send("All states configured!");
            
            IsConfigured = true;
            Configured?.Invoke();
            
            return Send("Configuration completed!");
        }

        public virtual IMessage Init()
        {
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);

            Subscribe();


            foreach (var state in States)
                state.Init();

            Execute<StateLoad>();

            Send("All states initialized!");

            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {
            Execute<StateUnload>();
            
            foreach (var state in States)
                state.Dispose();

            Send("All states disposed!");
            
            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
        }

        public virtual void Subscribe() { }
        public virtual void Unsubscribe() { }


        public void Execute<TState>()
        where TState : class, IState
        {
            if (m_StateActive != null)
                m_StateActive.Exit();

            if (СacheProvider<TState>.Contains())
            {
                m_StateActive = СacheProvider<TState>.Get();
                m_StateActive.Enter();
                m_StateActive.Run();
                StateChanged?.Invoke(m_StateActive);
                return;
            }

            Send($"{typeof(TState).Name} was not registered!", LogFormat.Worning);

        }
    }

    public struct StateControllerConfig : IConfig
    {
        public StateControllerConfig(IState[] states)
        {
            States = states;
        }

        public IState[] States { get; private set; }
    }

    public interface IStateController : IController, IConfigurable, IInitializable, ISubscriber, IMessager
    {
        event Action<IState> StateChanged;

        void Execute<TState>() 
        where TState : class, IState;
    }

}