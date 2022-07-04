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

        public override void Init()
        {
            var instance =  new Instance(this);
            var screens = new IScreen[3] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading"),
                m_Main = Set<ScreenMain>("Screen: Main"),
                m_Score = Set<ScreenScore>("Screen: Score")
            };

            var config =  new SceneConfig(instance, screens);
            
            Configure(config);
            base.Init();
        }

    }

}