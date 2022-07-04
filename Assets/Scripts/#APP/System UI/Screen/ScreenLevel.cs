using System;
using UnityEngine;
using APP.Button;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLevel : ScreenModel<ScreenLevel>, IScreen
    {
        //[SerializeField] private ButtonLevelPause m_Pause;
        //[SerializeField] private ButtonLevelResume m_Resume;
        //[SerializeField] private ButtonLevelExit m_Exit;

        public override void Init()
        {
            var buttons = new IButton[]
            {
                //m_Pause,
                //m_Resume,
                //m_Exit
            };

            var Instance = new Instance(this);
            var screenConfig = new ScreenConfig(Instance, buttons);
            
            base.Configure(screenConfig);
            base.Init();
        }

    }

}