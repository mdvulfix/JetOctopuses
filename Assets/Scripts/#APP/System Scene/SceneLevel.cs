using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneLevel : SceneModel<SceneLevel>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLevel m_Level_1;

        protected override void Init()
        {
            var screens = new IScreen[]
            {
                m_Loading,
                m_Level_1
            };

            var info = new InstanceInfo(this);
            var config = new SceneConfig(info, screens);
            base.Configure(config);
            base.Init();
        }

    }
}