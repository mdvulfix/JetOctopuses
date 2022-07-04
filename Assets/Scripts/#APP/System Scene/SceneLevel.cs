using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneLevel : SceneModel<SceneLevel>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLevel m_Level_1;

        public override void Init()
        {
            var instance =  new Instance(this);
            var screens = new IScreen[2] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading"),
                m_Level_1 = Set<ScreenLevel>("Screen: Level")
            };

            var config =  new SceneConfig(instance, screens);
            
            Configure(config);
            base.Init();
        }

    }
}