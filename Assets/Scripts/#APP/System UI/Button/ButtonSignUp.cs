using System;
using UnityEngine;
using APP;
using APP.Player;

namespace APP.UI
{
    [Serializable]
    public class ButtonSignUp : ButtonModel<ButtonSignUp>, IButton
    {
        [SerializeField] private PlayerAction m_PlayerAction;

        private readonly string m_Label = "Button: Sign Up";
        
        public ButtonSignUp() => Configure();
        public ButtonSignUp(IConfig config) => Configure(config);

        public void Configure()
        {             
            var config =  new ButtonConfig(m_Label, this);            
            base.Configure(config);
        }
    }

}