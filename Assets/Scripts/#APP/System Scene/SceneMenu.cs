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

        private readonly string m_Label = "Scene: Menu";
        private readonly SceneIndex m_SceneIndex = SceneIndex.Menu;

        public SceneMenu() => Configure();
        public SceneMenu(IConfig config) => Configure(config);

        public void Configure()
        {
            var screens = new List<IScreen>();
            screens.Add(m_Loading = new ScreenLoading(this));
            screens.Add(m_Main = new ScreenMain(this));
              
            var config =  new SceneConfig(this, m_SceneIndex, screens.ToArray(), m_Loading, m_Loading, m_Label);             
            base.Configure(config);
        }
    }

}