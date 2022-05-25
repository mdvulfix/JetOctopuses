using System;
using UnityEngine;

[Serializable]
public class ScreenMenu: ScreenModel<ScreenMenu>, IScreen
{ 

    [SerializeField] private ButtonMenuPlay m_ButtonPlay;
    [SerializeField] private ButtonMenuOptions m_ButtonOptions;
    [SerializeField] private ButtonMenuExit m_ButtonExit;


    protected override void Init() 
    {
        var buttons = new IButton[]
        {
            m_ButtonPlay,
            m_ButtonOptions,
            m_ButtonExit,
        };

        Configure(new ScreenConfig(this, buttons));
        base.Init();
    }
}

