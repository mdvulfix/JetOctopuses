using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneNet : SceneModel<SceneNet>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;

        public override void Init()
        {
            var instance =  new Instance(this);
            var screens = new IScreen[1] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading")
            };

            var config =  new SceneConfig(instance, screens);
            
            Configure(config);
            base.Init();
        }

    }
}