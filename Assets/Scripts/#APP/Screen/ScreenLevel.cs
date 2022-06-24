using System;
using UnityEngine;
using APP.Button;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLevel : ScreenModel<ScreenLevel>, IScreen
    {
        [SerializeField] private ButtonLevelPause m_Pause;
        [SerializeField] private ButtonLevelResume m_Resume;
        [SerializeField] private ButtonLevelExit m_Exit;

        protected override void Init()
        {
            var buttons = new IButton[]
            {
                m_Pause,
                m_Resume,
                m_Exit
            };

            Configure(new ScreenConfig(this, buttons));
            base.Init();
        }

    }

}