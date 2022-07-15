using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;


namespace APP.Scene
{
    public class SceneLogin : SceneModel<SceneLogin>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLogin m_Login;

        private readonly string m_Label = "Scene: Login";
        private readonly SceneIndex m_SceneIndex = SceneIndex.Login;

        public SceneLogin() => Configure();
        public SceneLogin(IConfig config) => Configure(config);


        public void Configure()
        {
            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading(this));
            screens.Add(m_Login = new ScreenLogin(this));
              
            var config =  new SceneConfig(this, m_SceneIndex, screens.ToArray(), m_Loading, m_Login, m_Label);            
            base.Configure(config);
        }
    }
}