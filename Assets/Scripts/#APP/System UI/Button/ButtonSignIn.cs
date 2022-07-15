using System;
using UnityEngine;
using APP.Player;

namespace APP.UI
{
    [Serializable]
    public class ButtonSignIn : ButtonModel<ButtonSignIn>, IButton
    {
        [SerializeField] private PlayerAction m_PlayerAction;

        private readonly string m_Label = "Button: Sign In";
        
        public ButtonSignIn() => Configure();
        public ButtonSignIn(IConfig config) => Configure(config);

        public void Configure()
        {             
            var config =  new ButtonConfig(m_Label, this);            
            base.Configure(config);
        }
    }

}