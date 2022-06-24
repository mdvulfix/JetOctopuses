using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneMenu : SceneModel<SceneMenu>, IScene
    {

        public static readonly SceneIndex Index = SceneIndex.Menu;

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

            Configure(new SceneConfig(this, Index, screens));
            base.Init();
        }

    }

}