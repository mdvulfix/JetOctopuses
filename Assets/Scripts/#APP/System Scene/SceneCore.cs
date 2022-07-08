using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{

    public class SceneCore : SceneModel<SceneCore>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenSplash m_Splash;
        
        private readonly string m_Label = "Scene: Core";

        public SceneCore() => Configure();
        public SceneCore(IConfig config) => Configure(config);

        public void Configure()
        {
            SceneIndex<SceneCore>.SetIndex(SceneIndex.Core);

            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading());
              
            var config =  new SceneConfig(m_Label, this, screens.ToArray());            
            base.Configure(config);
        }
    }
}
