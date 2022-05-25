using System;
using UnityEngine;

[Serializable]
public class ScreenLogin: ScreenModel<ScreenLogin>, IScreen
{

    [SerializeField] private ButtonSignIn m_ButtonSignIn;
    [SerializeField] private ButtonSignUp m_ButtonSignUp;

    protected override void Init()
    {
        var buttons = new IButton[]
        {
            m_ButtonSignIn,
            m_ButtonSignUp
        };

        Configure(new ScreenConfig(this, buttons));
        base.Init();
    }
}