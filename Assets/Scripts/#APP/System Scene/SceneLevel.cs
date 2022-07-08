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

        private readonly string m_Name = "Scene: Level";

        public SceneLevel() => Configure();
        public SceneLevel(IConfig config) => Configure(config);

        public void Configure()
        {
            SceneIndex<SceneLevel>.SetIndex(SceneIndex.Level);

            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading());
            screens.Add(m_Level_1 = new ScreenLevel());
            
            var config =  new SceneConfig(m_Name, this, screens.ToArray());            
            base.Configure(config);
        }
    }
}