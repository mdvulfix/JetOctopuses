using System;
using SERVICE.Factory;
using SERVICE.Handler;

namespace APP.Signal
{
    public abstract class SignalModel<TSignal> where TSignal : ISignal
    {
        private SignalConfig m_Config;
        private TSignal m_Signal;

        //private SignalProvider<TSignal> m_SignalProvider;

        public bool IsDebug { get; private set; }
        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public IConfig Config => m_Config;

        public event Action<ISignal> Initialized;
        public event Action<ISignal> Disposed;
        public event Action<ISignal> Called;

        public void Configure(IConfig config)
        {
            m_Config = (SignalConfig) config;
            m_Signal = (TSignal) m_Config.Signal;
            IsConfigured = true;
        }

        public virtual void Init()
        {
            if (IsConfigured == false)
            {
                Send("Configuration has not been done. Initialization aborted!", true);
                return;
            }

            //var signalProviderConfig = new SignalProviderConfig(m_Signal);

            //m_SignalProvider = new SignalProvider<TSignal>(signalProviderConfig);
            //m_SignalProvider.Init();

            IsDebug = true;
            IsInitialized = true;

            Initialized?.Invoke(m_Signal);

        }

        public virtual void Dispose()
        {
            Disposed?.Invoke(m_Signal);

            IsInitialized = false;

            //m_SignalProvider.Dispose();

        }

        public void Call()
        {
            if (IsInitialized)
            {
                Send("Signal called!");
                Called?.Invoke(m_Signal);

            }
            else
            {
                Send("Initialization has not been done. Calling aborted!", true);
                return;
            }

        }

        public string Send(string text, bool worning = false) =>
            LogHandler.Send(this, IsDebug, text, worning);

    }

    public class SignalConfig : Config
    {
        public ISignal Signal { get; }

        public SignalConfig(Instance info): base(info)
        {
        }

    }

    public class SignalFactory<TSignal> : IFactory
    where TSignal : UComponent, ISignal, IComparable
    {

        public TSignal Get()
        {
            Func<TSignal> creator = () => Activator.CreateInstance<TSignal>();
            return creator.Invoke();
        }

        public TSignal Get(IConfig config)
        {
            Func<TSignal> creator = () => Activator.CreateInstance<TSignal>();
            var signal = creator.Invoke();
            signal.Configure(config);

            return signal;
        }

        public T Get<T>(params object[] p) where T : UComponent, IConfigurable
        {
            throw new NotImplementedException();
        }
    }

}