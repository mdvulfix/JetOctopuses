using System;
using SERVICE.Handler;

namespace APP.Signal
{
    
    /*
    public class SignalProvider<TSignal> : SignalProvider
    where TSignal : class, ISignal
    {
        private ISignal m_Signal;
        
        public bool IsInitialized { get; private set; }

        public SignalProvider(ISignal signal)
        {
            m_Signal = signal;
        }


        public void Init()
        {

            m_Signal.Initialized += OnSignalInitialized;
            m_Signal.Disposed += OnSignalDisposed;
            m_Signal.Called += OnSignalCalled;

            IsDebug = true;
            IsInitialized = true;

        }

        public void Dispose()
        {
            IsInitialized = false;

            m_Signal.Initialized -= OnSignalInitialized;
            m_Signal.Disposed -= OnSignalDisposed;
            m_Signal.Called -= OnSignalCalled;

        }


        private void Set(IFactory<TSignal> factory, IConfig config)
        {
            var signal = factory.Get(config);
            m_Signals.Add(typeof(TSignal), signal);
        }


    }

    public class SignalProvider
    {
        public bool IsDebug { get; protected set; }

        public static event Action<ISignal> SignalInitialized;
        public static event Action<ISignal> SignalDisposed;
        public event Action<ISignal> SignalCalled;

        public string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send("Provider", IsDebug, text, worning);

        protected void OnSignalInitialized(ISignal signal)
        {
            SignalInitialized?.Invoke(signal);
            Send($"{signal} was initialized!");
        }

        protected void OnSignalDisposed(ISignal signal)
        {
            SignalDisposed?.Invoke(signal);
            Send($"{signal} was disposed!");
        }

        protected void OnSignalCalled(ISignal signal)
        {
            SignalCalled?.Invoke(signal);
            Send($"{signal} was called!");
        }

    }
    */
}