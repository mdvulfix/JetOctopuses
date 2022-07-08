using System;
using System.Collections.Generic;
using UnityEngine;
using APP.Screen;


namespace APP.Scene
{
    public class SceneMenu : SceneModel<SceneMenu>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenMain m_Main;
        [SerializeField] private ScreenScore m_Score;

        private readonly string m_Name = "Scene: Menu";

        public SceneMenu() => Configure();
        public SceneMenu(IConfig config) => Configure(config);

        public void Configure()
        {
            SceneIndex<SceneLogin>.SetIndex(SceneIndex.Login);

            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading());
            screens.Add(m_Main = new ScreenMain());
            screens.Add(m_Score = new ScreenScore());
              
            var config =  new SceneConfig(m_Name, this, screens.ToArray());            
            base.Configure(config);
        }
    }

}