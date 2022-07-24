using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{

    public class SceneCore : SceneModel<SceneCore>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLogin m_Splash;
        
        private readonly string m_Label = "Scene: Core";
        private readonly SceneIndex m_SceneIndex = SceneIndex.Core;

        public SceneCore() { }
        public SceneCore(IConfig config) => Configure(config);

        public void Configure()
        {
                    
            var screens = new List<IScreen>();
            //screens.Add(m_Loading = new ScreenLoading(this));
              
            var config =  new SceneConfig(this, m_SceneIndex, screens.ToArray(), m_Loading, m_Loading, m_Label);            
            base.Configure(config);
        }
    }
}
