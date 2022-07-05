using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenMain m_Main;
        [SerializeField] private ScreenScore m_Score;

        protected override void Init()
        {
            var screens = new IScreen[3] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading"),
                m_Main = Set<ScreenMain>("Screen: Main"),
                m_Score = Set<ScreenScore>("Screen: Score")
            };

            var config =  new SceneConfig<SceneMenu>(this, screens);
            
            Configure(config);
            base.Init();
        }

    }

}