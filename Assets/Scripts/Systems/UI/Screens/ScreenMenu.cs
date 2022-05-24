using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenMenu : ScreenModel<ScreenMenu>, IScreen
{

    [SerializeField] private Button m_Play;
    [SerializeField] private Button m_Options;
    [SerializeField] private Button m_Exit;
    
    
    public override void Init()
    {

    }

}

