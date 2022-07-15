using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;


namespace APP.Scene
{
    public class SceneLevel : SceneModel<SceneLevel>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLevel m_Level_1;

        private readonly string m_Label = "Scene: Level";
        private readonly SceneIndex m_SceneIndex = SceneIndex.Level;

        public SceneLevel() => Configure();
        public SceneLevel(IConfig config) => Configure(config);

        public void Configure()
        {
            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading(this));
            screens.Add(m_Level_1 = new ScreenLevel(this));
            
            var config =  new SceneConfig(this, m_SceneIndex, screens.ToArray(), m_Loading, m_Loading, m_Label);           
            base.Configure(config);
        }
    }
}