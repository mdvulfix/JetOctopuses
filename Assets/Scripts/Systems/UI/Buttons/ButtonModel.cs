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

    public void Configure (IConfig config)
    {
        m_Config = (ButtonConfig) config;
        IsConfigured = true;
    }

    public virtual void Init ()
    {
        if (IsConfigured == false)
        {
            Send ("Configuration has not been done. Initialization aborted!", true);
            return;
        }

        //m_Signal = new SignalAction();
        //m_Signal.Configure(new SignalConfig<TActionInfo>(m_Signal, m_ActionInfo));
        //m_Signal.Init();

        Subscribe ();

        IsDebug = true;
        IsInitialized = true;

    }

    public virtual void Dispose ()
    {
        IsInitialized = false;

        Unsubscribe ();

        //m_Signal.Dispose();

    }

    public string Send (string text, bool worning = false) =>
        Messager.Send (this, IsDebug, text, worning);

    protected void Subscribe () =>
        onClick.AddListener (() => ButtonClick ());

    protected void Unsubscribe () =>
        onClick.RemoveListener (() => ButtonClick ());

    private void ButtonClick ()
    {
        //m_Signal.Call();
        //ButtonClicked?.Invoke(m_Signal);
    }

}

[Serializable]
public class ButtonSignUp : ButtonModel<ButtonSignUp>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonSignIn : ButtonModel<ButtonSignIn>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonMenuPlay : ButtonModel<ButtonMenuPlay>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }

}

[Serializable]
public class ButtonMenuOptions : ButtonModel<ButtonMenuOptions>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonOptionsExit : ButtonModel<ButtonOptionsExit>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonMenuExit : ButtonModel<ButtonMenuExit>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonMenuLevel : ButtonModel<ButtonMenuLevel>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;
    //[SerializeField] private LevelIndex m_LevelIndex;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        //if (m_LevelIndex == LevelIndex.None)
        //{
        //    Send ("Level not assigned!", true);
        //    return;
        //}

        //var info = new ActionLevelLoadInfo (m_PlayerAction, m_LevelIndex);
        //var config = new ButtonConfig<ActionInfo> (this, info);

        //Configure (config);
        //base.Init ();
    }

}

[Serializable]
public class ButtonLevelPlay : ButtonModel<ButtonLevelPlay>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonLevelPause : ButtonModel<ButtonLevelPause>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

[Serializable]
public class ButtonLevelExit : ButtonModel<ButtonLevelExit>, IButton
{
    [SerializeField] private PlayerAction m_PlayerAction;

    public override void Init ()
    {
        if (m_PlayerAction == PlayerAction.None)
        {
            Send ("Player action not assigned!", true);
            return;
        }

        var config = new ButtonConfig (this);

        Configure (config);
        base.Init ();
    }
}

public interface IButton : IConfigurable
{
    event Action<ISignal> ButtonClicked;
}


public struct ButtonConfig : IConfig
{
    public IButton Button { get; }

    public ButtonConfig (IButton button)
    {
        Button = button;
    }

}