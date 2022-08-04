using System;
using UnityEngine;

namespace APP.UI
{
    [Serializable]
    public class ButtonSignUp : ButtonModel<ButtonSignUp>, IButton
    {
        [SerializeField] private UserAction m_UserAction;

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