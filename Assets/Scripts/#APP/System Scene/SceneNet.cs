using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneNet : SceneModel, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;

        protected override void Init()
        {
            var screens = new IScreen[1] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading")
            };

            var config =  new SceneConfig<SceneNet>(this, screens);
            
            Configure(config);
            base.Init();
        }

    }
}