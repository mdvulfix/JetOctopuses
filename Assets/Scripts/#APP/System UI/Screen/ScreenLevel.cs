using System;
using System.Collections.Generic;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLevel : ScreenModel<ScreenLevel>, IScreen
    {
        //[SerializeField] private ButtonLevelPause m_Pause;
        //[SerializeField] private ButtonLevelResume m_Resume;
        //[SerializeField] private ButtonLevelExit m_Exit;

        private readonly string m_Label = "Screen: Level";

        public ScreenLevel(IScene scene) => Configure(scene);
        public ScreenLevel(IConfig config) => Configure(config);

        public void Configure(IScene scene)
        {
            var buttons = new List<IButton>();
            //screens.Add(m_Loading = new ScreenLoading());
              
            var config =  new ScreenConfig(this, scene, buttons.ToArray(), m_Label);            
            base.Configure(config);
        }
    }

}