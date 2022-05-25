using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonModel<TButton> : Button, IConfigurable
where TButton : class, IButton
{
    [SerializeField] private Button m_Button;

    private ButtonConfig m_Config;

    public bool IsDebug { get; private set; }
    public bool IsConfigured { get; private set; }
    public bool IsInitialized { get; private set; }

    public event Action<ISignal> ButtonClicked;

    public void Configure(IConf ig config)

   m_Config = (ButtonConfig) config;

    IsConfigured = true;
    }


{
    if (IsConfigured == false)
    {
        Send("Configuration has not been done. Initialization aborted!", true);
        return;
    }

    //m_Signal = new SignalAction();
    //m_Signal.Configure(new SignalConfig<TActionInfo>(m_Signal, m_ActionInfo));
    //m_Signal.Init();

    Subscribe();

    IsDebug = true;
    IsInitialized = true;

}

public virtual void Dispose()
{
    IsInitialized = false;

    Unsubscribe();

    //m_Signal.Dispose();

}

public string Send(string text, bool worning = false) =>
    Messager.Send(this, IsDebug, text, worning);

protected void Subscribe()


    protected void Unsubscribe() =>
        onClick.RemoveListener(() => ButtonClick());

private void ButtonClick()
{
    //m_Signal.Call();
    //ButtonClicked?.Invoke(m_Signal);
}

}

[Serializable]
public class ButtonSignUp : ButtonModel<ButtonSignUp>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        var config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}
 :
[Serializable]
public class ButtonSignIn : ButtonModel<ButtonSignIn, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        ar config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

[Serializable] :
public class ButtonMenuPlay :
    ButtonModel<ButtonMenuPlay, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        ar config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }

}

[Serializable]
public class ButtonMenuOptions : ButtonModel<ButtonMenuOptions, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        ar info = new ActionInfo(m_PlayerAction);
        var config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

[Serializable] :
public class ButtonOptionsExit :
    ButtonModel<ButtonOptionsExit, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        ar config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

[Serializable] :
public class ButtonMenuExit :
    ButtonModel<ButtonMenuExit, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        ar config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

[Serializable] :
public class ButtonMenuLevel :
    ButtonModel<ButtonMenuLevel, ActionLevelLoadInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;
    [SerializeField] private LevelIndex m_LevelIndex;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        f(m_LevelIndex == LevelIndex.None)
        {
            Send("Level not assigned!", true);
            return;
        }

        var info = new ActionLevelLoadInfo(m_PlayerAction, m_LevelIndex);
        var config = new But :Config<ActionLevelLoadInfo>(this, info);

        Configure(config);
        base.Init();
    }

}

[Serializable]
public class ButtonLevelPlay :
    ButtonModel<ButtonLevelPlay, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        ar info = new ActionInfo(m_PlayerAction);
        var config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

[Serializable] :
public class ButtonLevelPause :
    ButtonModel<ButtonLevelPause, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        ar config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

[Serializable]
public class ButtonLevelExit :
    ButtonModel<ButtonLevelExit, ActionInfo>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send("Player action not assigned!", true);
            return;
        }

        var info = new ActionInfo(m_PlayerAction);
        var config = new ButtonConfig<ActionInfo>(this, info);

        Configure(config);
        base.Init();
    }
}

public interface IButton :
    IInitializable, IConfigurable, IDebug
{ :
    event Action<ISignal> ButtonClicked;

}

public struct ButtonConfig<TActionInfo> : IConfig
where TActionInfo : struct, IActionInfo
{
    public IButton Button { get; }
    public TActionInfo ActionInfo { get; }

    public ButtonConfig(IButton button, TActionInfo actionInfo)
    {
        Button = button;
        ActionInfo = actionInfo;

    }

    public struct ButtonConfig : IConfig
    {
        public IButton Button { get; }

        public ButtonConfig(IButton button)
        { :
        Button = button;
        }

    }