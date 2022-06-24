using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneCore : SceneModel<SceneCore>, IScene
    {
        public static readonly SceneIndex Index = SceneIndex.Core;

        [SerializeField] private ScreenLoading m_Loading;

        protected override void Init()
        {
            var screens = new IScreen[]
            {
                m_Loading
            };

            Configure(new SceneConfig(this, Index, screens));
            base.Init();
        }

    }
}