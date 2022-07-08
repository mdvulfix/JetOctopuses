using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;


namespace APP.Scene
{
    public class SceneLogin : SceneModel<SceneLogin>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;

        private readonly string m_Name = "Scene: Login";

        public SceneLogin() => Configure();
        public SceneLogin(IConfig config) => Configure(config);

        public void Configure()
        {
            SceneIndex<SceneLogin>.SetIndex(SceneIndex.Login);

            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading());
              
            var config =  new SceneConfig(m_Name, this, screens.ToArray());            
            base.Configure(config);
        }
    }
}