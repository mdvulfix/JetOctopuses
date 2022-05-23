using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenMainMenu : AScreen
{

    [SerializeField] private Button m_Play;
    [SerializeField] private Button m_Options;
    [SerializeField] private Button m_Exit;
    
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


}

