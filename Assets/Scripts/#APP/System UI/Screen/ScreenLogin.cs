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

        public ScreenLogin(params object[] param) 
        => Configure(param);
        
        public ScreenLogin(IScene scene, string label = "Screen: Login")
        {
            var buttons = new List<IButton>();
            buttons.Add(m_SignIn = new ButtonSignIn());
            buttons.Add(m_SignUp = new ButtonSignUp());
              
            var screenConfig =  new ScreenConfig(this, scene, buttons.ToArray(), label);            
            Configure(screenConfig);
        }
    }
}