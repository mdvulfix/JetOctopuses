using System;

public abstract class SignalModel<TSignal, TActionInfo>
    where TSignal: class, ISignal<TActionInfo>
    where TActionInfo: struct, IActionInfo
{
    private SignalConfig<TActionInfo> m_Config;
    private TSignal m_Signal;
    private TActionInfo m_ActionInfo;
    
    private SignalProvider<TSignal> m_SignalProvider;


    public bool IsDebug { get; private set; }
    public bool IsConfigured {get; private set; }
    public bool IsInitialized {get; private set; }

    public IConfig Config => m_Config;
    public TActionInfo ActionInfo => m_ActionInfo;

    
    public event Action<ISignal> Initialized;
    public event Action<ISignal> Disposed;
    public event Action<ISignal> Called;  
    
    public void Configure(IConfig config)
    {
        m_Config = (SignalConfig<TActionInfo>)config;
        m_Signal = (TSignal)m_Config.Signal;
        m_ActionInfo = m_Config.ActionInfo;
        

        IsConfigured = true;
    }

    public virtual void Init()
    {
        if (IsConfigured == false)
        {
            Send("Configuration has not been done. Initialization aborted!", true);
            return;
        }

        var signalProviderConfig = new SignalProviderConfig(m_Signal);

        m_SignalProvider = new SignalProvider<TSignal>(signalProviderConfig);
        m_SignalProvider.Init();
        
        
        IsDebug = true;
        IsInitialized = true;

        Initialized?.Invoke(m_Signal);

    }

    public virtual void Dispose()
    {
        Disposed?.Invoke(m_Signal);

        IsInitialized = false;

        m_SignalProvider.Dispose();

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
        Messager.Send(this, IsDebug, text, worning);

}


public class SignalAction: 
    SignalModel<SignalAction, ActionInfo>, 
    ISignal<ActionInfo>
{
    public SignalAction()
    {
    }

    public SignalAction(IConfig config)
    {
        Configure(config);
        base.Init();
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

public interface ISignal<TActionInfo>: ISignal
    where TActionInfo: struct, IActionInfo
{
    TActionInfo ActionInfo {get; }
}

public interface ISignal: IInitializable, IConfigurable, IDebug
{
    event Action<ISignal> Initialized;
    event Action<ISignal> Disposed;
    event Action<ISignal> Called;

    void Call();

}


public struct ActionInfo : IActionInfo
{
    public PlayerAction PlayerAction { get;  }

    public ActionInfo(PlayerAction playerAction)
    {
        PlayerAction = playerAction;
    }
}

public struct ActionLevelLoadInfo : IActionInfo
{
    public PlayerAction PlayerAction { get;  }
    public LevelIndex LevelIndex { get;  }
    
    public ActionLevelLoadInfo(PlayerAction playerAction, LevelIndex levelIndex)
    {
        PlayerAction = playerAction;
        LevelIndex = levelIndex;

    }
}

public interface IActionInfo
{ 


}


public struct SignalConfig<TActionInfo>: IConfig
    where TActionInfo: struct, IActionInfo
{
    public ISignal Signal  {get; }
    public TActionInfo ActionInfo  {get; }

    public SignalConfig(ISignal signal, TActionInfo actionInfo)
    {
        Signal = signal;
        ActionInfo = actionInfo;
    }
}

public struct SignalConfig: IConfig
{
    public ISignal Signal  {get; }

    public SignalConfig(ISignal signal)
    {
        Signal = signal;
    }

}



public class SignalFactory<TSignal> : IFactory<TSignal>
    where TSignal : class, ISignal
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

}