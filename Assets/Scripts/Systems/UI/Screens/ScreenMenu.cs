using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenMenu : AScreen<ScreenMenu>
{

    [SerializeField] private Button m_Play;
    [SerializeField] private Button m_Options;
    [SerializeField] private Button m_Exit;
    
    
    protected override void Init()
    {

    }

}

