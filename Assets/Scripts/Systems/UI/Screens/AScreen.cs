using System;
using UnityEngine;

public abstract class AScreen<TScreen>: Cachable<TScreen>
{
    public event Action ScreenActivated;
    public event Action ScreenAnimated;

    private void Awake() 
    {
        Init();
    }

    protected virtual void OnEnable()
    {
        Set();
    }

    protected virtual void OnDisable()
    {
        Del();
    }



    protected abstract void Init();

    protected void Activate(bool activate)
    {
        if(activate)
            ScreenActivated?.Invoke();
    }

    protected void Animate(bool animate)
    {
        if(animate)
            ScreenAnimated?.Invoke();   
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

public interface IScreen
{
    

}