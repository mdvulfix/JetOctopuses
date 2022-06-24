using System;
using UnityEngine;
using APP.Button;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLogin : ScreenModel<ScreenLogin>, IScreen
    {

        [SerializeField] private ButtonSignIn m_SignIn;
        [SerializeField] private ButtonSignUp m_SignUp;

        protected override void Init()
        {
            var buttons = new IButton[]
            {
                m_SignIn,
                m_SignUp
            };

            Configure(new ScreenConfig(this, buttons));
            base.Init();
        }
    }
}