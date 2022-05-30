using System;
using System.Collections.Generic;

public class SignalProvider<TSignal>: SignalProvider
    where TSignal: class, ISignal
{
    private SignalProviderConfig m_Config;
    private ISignal m_Signal;


    public bool IsConfigured {get; private set;}
    public bool IsInitialized {get; private set;}

    public IConfig Config => m_Config;

    public SignalProvider(SignalProviderConfig config)
    {
        Configure(config);
    }

    public void Configure(IConfig config)
    {
        m_Config = (SignalProviderConfig)config;
        m_Signal = m_Config.Signal;
        
        IsConfigured = true;
    }

    public void Init()
    {
        if (IsConfigured == false)
        {
            Send("Configuration has not been done. Initialization aborted!", true);
            return;
        }

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

    /*
    private void Set(IFactory<TSignal> factory, IConfig config)
    {
        var signal = factory.Get(config);
        m_Signals.Add(typeof(TSignal), signal);
    }
    */




}


public class SignalProvider
{   
    public bool IsDebug { get; protected set; }

    public static event Action<ISignal> SignalInitialized;
    public static event Action<ISignal> SignalDisposed;
    public event Action<ISignal> SignalCalled;



    public string Send(string text, bool worning = false) =>
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

public class SignalProviderConfig: IConfig
{
    public ISignal Signal {get; private set; }

    public SignalProviderConfig(ISignal signal)
    {
        Signal = signal;
    }
}

