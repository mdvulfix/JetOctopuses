using System;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLevel : ScreenModel<ScreenLevel>, IScreen
    {
        //[SerializeField] private ButtonLevelPause m_Pause;
        //[SerializeField] private ButtonLevelResume m_Resume;
        //[SerializeField] private ButtonLevelExit m_Exit;

        protected override void Init()
        {
            var buttons = new IButton[]
            {
                //m_Pause,
                //m_Resume,
                //m_Exit
            };


            var screenConfig = new ScreenConfig(this, buttons);
            
            base.Configure(screenConfig);
            base.Init();
        }

    }

}