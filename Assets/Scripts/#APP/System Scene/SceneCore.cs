using System;
using UnityEngine;
using APP;
using APP.Screen;
using SERVICE.Handler;

namespace APP.Scene
{
    [Serializable]
    public class SceneCore : SceneModel<SceneCore>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenSplash m_Splash;
        
        private readonly string m_Name = "Scene: Core";

        public SceneCore() => 
            Configure();

        public override void Configure()
        {
            var screens = new IScreen[2] 
            {
                m_Loading = SetComponent<ScreenLoading>("Screen: Loading"),
                m_Splash = SetComponent<ScreenSplash>("Screen: Splash")
            };
            
            var config =  new SceneConfig(m_Name, this, screens);            
            base.Configure(config);
        }
    }
}
