using System;
using UnityEngine;
using APP.Button;

namespace APP.Screen
{
    [Serializable]
    public class ScreenMain : ScreenModel<ScreenMain>, IScreen
    {
        //[SerializeField] private ButtonMenuPlay m_Play;
        //[SerializeField] private ButtonMenuOptions m_Options;
        //[SerializeField] private ButtonMenuExit m_Exit;

        public override void Init()
        {
            var buttons = new IButton[]
            {
                //m_Play,
                //m_Options,
                //m_Exit,
            };

            var info = new Instance(this);
            var config = new ScreenConfig(info, buttons);
            
            base.Configure(config);
            base.Init();
        }
    }

}