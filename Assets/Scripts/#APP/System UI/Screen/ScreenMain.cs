using System;
using System.Collections.Generic;

namespace APP.Screen
{
    [Serializable]
    public class ScreenMain : ScreenModel<ScreenMain>, IScreen
    {
        //[SerializeField] private ButtonMenuPlay m_Play;
        //[SerializeField] private ButtonMenuOptions m_Options;
        //[SerializeField] private ButtonMenuExit m_Exit;

        public ScreenMain(params object[] param) 
        => Configure(param);
        
        public ScreenMain(IScene scene, string label = "Screen: Main")
        {
            var buttons = new List<IButton>();

              
            var screenConfig =  new ScreenConfig(this, scene, buttons.ToArray(), label);            
            Configure(screenConfig);
        }
    }

}