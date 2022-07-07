using System;

namespace APP
{
    public class StateControllerDefault : Controller, IStateController
    {
        private StateControllerConfig m_Config;

        private IState m_StateActive;

        public StateControllerDefault() => Configure();
        public StateControllerDefault(IConfig config) => Configure(config);

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<IState> StateChanged;


        // CONFIGURE //
        public virtual void Configure() =>
            Configure(config: null);

        public virtual void Configure(IConfig config) =>
            Configure(config: config, param: null);

        public virtual void Configure (IConfig config, params object[] param)
        {
            if(config != null)
            {
                m_Config = (StateControllerConfig) config;

            }          
               
            if(param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            OnConfigured();
        }


        public void Init()
        {
            if (IsConfigured == false)
            {
                Send("Instance was not configured. Initialization was failed!", LogFormat.Worning);
                return;
            }

            OnInitialized();
        }

        public void Dispose()
        {

            OnDisposed();
        }

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

    }

    public struct StateControllerConfig : IConfig
    {

    }

    public interface IStateController : IController, IConfigurable, IInitializable
    {
        event Action<IState> StateChanged;

        void Execute<TState>() 
        where TState : class, IState;
    }

}