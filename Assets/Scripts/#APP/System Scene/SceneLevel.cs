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

        private readonly string m_Name = "Scene: Level";

        public SceneLevel() => 
            Configure();

        public override void Configure()
        {
            var screens = new IScreen[2] 
            {
                m_Loading = SetComponent<ScreenLoading>("Screen: Loading"),
                m_Level_1 = SetComponent<ScreenLevel>("Screen: Level")
            };
            
            var config =  new SceneConfig(m_Name, this, screens);            
            base.Configure(config);
        }
    }
}