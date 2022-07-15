using System;
using UnityEngine;
using APP.UI;
using System.Collections.Generic;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLogin : ScreenModel<ScreenLogin>, IScreen
    {

        [SerializeField] private ButtonSignIn m_SignIn;
        [SerializeField] private ButtonSignUp m_SignUp;

        private readonly string m_Label = "Screen: Login";

        public ScreenLogin(IScene scene) => Configure(scene);
        public ScreenLogin(IConfig config) => Configure(config);

        public void Configure(IScene scene)
        {
            var buttons = new List<IButton>();
            buttons.Add(m_SignIn = new ButtonSignIn());
            buttons.Add(m_SignUp = new ButtonSignUp());
              
            var config =  new ScreenConfig(this, scene, buttons.ToArray(), m_Label);            
            base.Configure(config);
        }
    }
}