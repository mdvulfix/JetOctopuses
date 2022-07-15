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

        private readonly string m_Label = "Screen: Main";

        public ScreenMain(IScene scene) => Configure(scene);
        public ScreenMain(IConfig config) => Configure(config);

        public void Configure(IScene scene)
        {
            var buttons = new List<IButton>();

              
            var config =  new ScreenConfig(this, scene, buttons.ToArray(), m_Label);            
            base.Configure(config);
        }
    }

}