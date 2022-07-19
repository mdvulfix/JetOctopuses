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

        public ScreenLevel(params object[] param) 
        => Configure(param);
        
        public ScreenLevel(IScene scene, string label = "Screen: Level")
        {
            var buttons = new List<IButton>();
            //screens.Add(m_Loading = new ScreenLoading());
              
            var screenConfig =  new ScreenConfig(this, scene, buttons.ToArray(), label);            
            Configure(screenConfig);
        }
        
        
        
    }

}