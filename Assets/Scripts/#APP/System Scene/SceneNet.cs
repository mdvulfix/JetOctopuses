using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;


namespace APP.Scene
{
    public class SceneNet : SceneModel<SceneNet>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        
        private readonly string m_Name = "Scene: Net";

        public SceneNet() => Configure();
        public SceneNet(IConfig config) => Configure(config);

        public void Configure()
        {
            SceneIndex<SceneNet>.SetIndex(SceneIndex.Net);

            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading());
              
            var config =  new SceneConfig(m_Name, this, screens.ToArray());            
            base.Configure(config);
        }
    }
}