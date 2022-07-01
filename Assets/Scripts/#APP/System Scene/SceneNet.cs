using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneNet : SceneModel<SceneNet>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;

        protected override void Init()
        {
            var screens = new IScreen[]
            {
                m_Loading,
            };

            var info = new InstanceInfo(this);
            var config = new SceneConfig(info, screens);
            base.Configure(config);
            base.Init();
        }

    }
}