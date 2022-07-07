using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel<SceneMenu>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenMain m_Main;
        [SerializeField] private ScreenScore m_Score;

        private readonly string m_Name = "Scene: Menu";

        public SceneMenu() => 
            Configure();

        public override void Configure()
        {
            var screens = new IScreen[3] 
            {
                m_Loading = SetComponent<ScreenLoading>("Screen: Loading"),
                m_Main = SetComponent<ScreenMain>("Screen: Main"),
                m_Score = SetComponent<ScreenScore>("Screen: Score")
            };
            
            var config =  new SceneConfig(m_Name, this, screens);            
            base.Configure(config);
        }
    }

}