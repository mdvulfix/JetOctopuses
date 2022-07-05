using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneLevel : SceneModel, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLevel m_Level_1;

        protected override void Init()
        {
            var screens = new IScreen[2] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading"),
                m_Level_1 = Set<ScreenLevel>("Screen: Level")
            };

            var config =  new SceneConfig<SceneLevel>(this, screens);
            
            Configure(config);
            base.Init();
        }

    }
}