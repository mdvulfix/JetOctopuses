using System;

public abstract class SignalModel<TSignal>
    where TSignal : class, ISignal
    {
        private SignalConfig m_Config;
        private TSignal m_Signal;

        private SignalProvider<TSignal> m_SignalProvider;

        public bool IsDebug { get; private set; }
        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public IConfig Config => m_Config;

        public event Action<ISignal> Initialized;
        public event Action<ISignal> Disposed;
        public event Action<ISignal> Called;

        public void Configure (IConfig config)
        {
            m_Config = (SignalConfig) config;
            m_Signal = (TSignal) m_Config.Signal;
            IsConfigured = true;
        }

        public virtual void Init ()
        {
            if (IsConfigured == false)
            {
                Send ("Configuration has not been done. Initialization aborted!", true);
                return;
            }

            var signalProviderConfig = new SignalProviderConfig (m_Signal);

            m_SignalProvider = new SignalProvider<TSignal> (signalProviderConfig);
            m_SignalProvider.Init ();

            IsDebug = true;
            IsInitialized = true;

            Initialized?.Invoke (m_Signal);

        }

        public virtual void Dispose ()
        {
            Disposed?.Invoke (m_Signal);

            IsInitialized = false;

            m_SignalProvider.Dispose ();

        }

        public void Call ()
        {
            if (IsInitialized)
            {
                Send ("Signal called!");
                Called?.Invoke (m_Signal);

            }
            else
            {
                Send ("Initialization has not been done. Calling aborted!", true);
                return;
            }

        }

        public string Send (string text, bool worning = false) =>
            Messager.Send (this, IsDebug, text, worning);

    }

public class SignalAction : SignalModel<SignalAction>, ISignal
{
    public SignalAction () { }

    public SignalAction (IConfig config)
    {
        Configure (config);
        base.Init ();
    }
}

/*
public class SignalMenuPlay: 
    SignalModel<SignalMenuPlay, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuPlay()
    {
        var signalInfo = new SignalInfo(this, PlayerAction.MenuPlay);
        Configure(new SignalConfig<SignalInfo>(signalInfo));
        base.Init();
    }
}

public class SignalMenuMain: 
    SignalModel<SignalMenuMain, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuMain()
    {
        var signalInfo = new SignalInfo(this, PlayerAction.MenuMain);
        Configure(new SignalConfig<SignalInfo>(signalInfo));
        base.Init();
    }

}


public class SignalMenuOptions: 
    SignalModel<SignalMenuOptions, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuOptions(SignalConfig<SignalInfo> info)
    {
        Configure(info);
        base.Init();
    }

}

public class SignalMenuExit: 
    SignalModel<SignalMenuExit, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuExit(SignalConfig<SignalInfo> info)
    {
        Configure(info);
        base.Init();
    }

}


public class SignalMenuLevel:
    SignalModel<SignalMenuLevel, SignalLevelInfo>,
    ISignal<SignalLevelInfo>
{

    public SignalMenuLevel(SignalConfig<SignalLevelInfo> info)
    {
        Configure(info);
        base.Init();
    }
}

*/

public interface ISignal : IConfigurable
{
    event Action<ISignal> Initialized;
    event Action<ISignal> Disposed;
    event Action<ISignal> Called;

    void Call ();

}

public struct SignalConfig : IConfig
{
    public ISignal Signal { get; }

    public SignalConfig (ISignal signal)
    {
        Signal = signal;
    }

}

public class SignalFactory<TSignal> : IFactory
where TSignal : class, ISignal
{

    public TSignal Get ()
    {
        Func<TSignal> creator = () => Activator.CreateInstance<TSignal> ();
        return creator.Invoke ();
    }

    public TSignal Get (IConfig config)
    {
        Func<TSignal> creator = () => Activator.CreateInstance<TSignal> ();
        var signal = creator.Invoke ();
        signal.Configure (config);

        return signal;
    }

}

public interface IFactory
{

}