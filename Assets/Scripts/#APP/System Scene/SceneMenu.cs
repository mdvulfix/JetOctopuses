using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel<SceneMenu>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenLogin m_Login;
        [SerializeField] private ScreenMain m_Main;
        [SerializeField] private ScreenScore m_Score;

        protected override void Init()
        {
            var screens = new IScreen[]
            {
                m_Loading,
                m_Login,
                m_Main,
                m_Score
            };

            var info = new InstanceInfo(this);
            var config = new SceneConfig(info, screens);
            base.Configure(config);
            base.Init();
        }

    }

}