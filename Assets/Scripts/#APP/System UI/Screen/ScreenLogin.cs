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

        public override void Init()
        {
            var buttons = new IButton[]
            {
                m_SignIn,
                m_SignUp
            };

            var info = new Instance(this);
            var config = new ScreenConfig(info, buttons);
            
            base.Configure(config);
            base.Init();
        }
    }
}