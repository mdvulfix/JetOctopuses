using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ScreenModel<TScreen> : SceneObject, IConfigurable
where TScreen : SceneObject, IScreen
{
    public event Action ScreenActivated;
    public event Action ScreenAnimated;


    private ScreenConfig m_Config;
    private Register<TScreen> m_Register;
    private ScreenController m_ScreenController;

    private void Awake ()
    {

    }

    private void OnEnable ()
    {
        m_Register.Add((TScreen)m_Config.Screen);
        Init ();
    }

    private void OnDisable ()
    {
        Dispose ();
        m_Register.Remove((TScreen)m_Config.Screen);
    }

    public virtual void Configure (IConfig config)
    {
        m_Config = (ScreenConfig) config;

        var screenControllerConfig = new ScreenControllerConfig ();
        m_ScreenController = new ScreenController (screenControllerConfig);
    }

    public virtual void Init ()
    {
        m_ScreenController.Init ();
    }

    public virtual void Dispose ()
    {
        m_ScreenController.Dispose ();
    }


    /*
    public event Action<IEventArgs> PlayButtonClicked;
    public event Action<IEventArgs> OptionsButtonClicked;
    public event Action<IEventArgs> ExitButtonClicked;

    protected override void OnAwake()
    {

    }

    protected override void OnEnable()
    {
        m_Play.onClick.AddListener(() => Play());
        m_Options.onClick.AddListener(() => Options());
        m_Exit.onClick.AddListener(() => Exit());
        
        Add(this);

    }

    protected override void OnDisable()
    {
        m_Play.onClick.RemoveListener(() => Play());
        m_Options.onClick.RemoveListener(() => Options());
        m_Exit.onClick.RemoveListener(() => Exit());

        Remove(this);
    }
    
    
    private void Play(IEventArgs args = null)
    {
        PlayButtonClicked?.Invoke(args);
    }

    private void Options(IEventArgs args = null)
    {
        OptionsButtonClicked?.Invoke(args);
    }

    private void Exit(IEventArgs args = null)
    {
        ExitButtonClicked?.Invoke(args);
    }

    */

}

public struct ScreenConfig : IConfig
{
    public IScreen Screen {get; private set; }

    public ScreenConfig (IScreen screen)
    {
        Screen = screen;
    }
}

public interface IScreen : IConfigurable
{

}