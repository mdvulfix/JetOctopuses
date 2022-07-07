using System;
using UnityEngine;
using APP.UI;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLogin : ScreenModel<ScreenLogin>, IScreen
    {

        [SerializeField] private ButtonSignIn m_SignIn;
        [SerializeField] private ButtonSignUp m_SignUp;

        public override void Init()
        {
            var buttons = new IButton[]
            {
                m_SignIn,
                m_SignUp
            };

            var config = new ScreenConfig(this, buttons);
            
            base.Configure(config);
            base.Init();
        }
    }
}