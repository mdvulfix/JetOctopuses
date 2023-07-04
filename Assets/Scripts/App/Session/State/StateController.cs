using System;
using System.Collections.Generic;
using Core;
using Core.Cache;

namespace App
{

    /*
    public class StateController : AController, IStateController
    {
        private StateControllerConfig m_Config;

        private IState m_StateActive;
        private IState m_StateLoad;
        private IState m_StateUnload;

        public IState[] States { get; private set; }

        public event Action<IState> StateChanged;

        public StateControllerDefault(IConfig config) => Configure(config);
        public StateControllerDefault() { }

        // CONFIGURE //
        public override void Configure(params object[] param)
        {
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Warning);
                return;
            }

            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (StateControllerConfig)obj;
                        Send($"{obj.GetName()} was setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Warning);
            }


            //IsConfigured = true;
            //Configured?.Invoke();

            Send("Configuration completed!");
        }

        public override void Init()
        {
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Warning);
                return;
            }

            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Warning);
                return;
            }

            Subscribe();

            //IsInitialized = true;
            //Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public override void Dispose()
        {

            Unsubscribe();

            //IsInitialized = false;
            //Disposed?.Invoke();
            Send("Dispose completed!");
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

            Send($"{typeof(TState).Name} was not registered!", LogFormat.Warning);
           
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

    public interface IStateController : IController, IConfigurable, ISubscriber, IMessager
    {
        event Action<IState> StateChanged;

        void Execute<TState>()
        where TState : class, IState;
    }
    */

}