using System;
using System.Collections.Generic;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLoading : ScreenModel<ScreenLoading>, IScreen
    {
        
        
        public ScreenLoading(params object[] param) 
        => Configure(param);
        
        public ScreenLoading(IScene scene, string label = "Screen: Loading")
        {
            var buttons = new List<IButton>();
            //screens.Add(m_Loading = new ScreenLoading());
              
            var screenConfig =  new ScreenConfig(this, scene, buttons.ToArray(), label);            
            Configure(screenConfig);
        }

        
    }
}