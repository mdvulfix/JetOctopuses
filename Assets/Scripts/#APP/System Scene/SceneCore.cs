using System;
using UnityEngine;
using APP;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneCore : SceneModel, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenSplash m_Splash;

        
        protected override void Init()
        {
            var screens = new IScreen[2] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading"),
                m_Splash = Set<ScreenSplash>("Screen: Splash")
            };

            var config =  new SceneConfig<SceneCore>(this, screens);
            
            Configure(config);
            base.Init();
        }
    }
}
