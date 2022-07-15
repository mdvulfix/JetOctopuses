using System;
using System.Collections.Generic;

namespace APP.Screen
{
    [Serializable]
    public class ScreenLoading : ScreenModel<ScreenLoading>, IScreen
    {
        private readonly string m_Label = "Screen: Loading";

        public ScreenLoading(IScene scene) => Configure(scene);
        public ScreenLoading(IConfig config) => Configure(config);

        public void Configure(IScene scene)
        {
            var buttons = new List<IButton>();
            //screens.Add(m_Loading = new ScreenLoading());
              
            var config =  new ScreenConfig(this, scene, buttons.ToArray(), m_Label);            
            base.Configure(config);
        }

    }
}