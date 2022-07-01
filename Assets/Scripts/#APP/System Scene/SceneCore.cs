using System;
using UnityEngine;

using SERVICE.Builder;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneCore : SceneModel<SceneCore>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenSplash m_Splash;

        
        public override void Configure(IConfig config)
        {
            if(ConfigValidate())
                return;
            
            var sceneConfig = (SceneConfig) config;
            var screens = sceneConfig.Screens;
            
            SetScreen<ScreenLoading>(ref m_Loading, screens);
            SetScreen<ScreenSplash>(ref m_Splash, screens);

            base.Configure(config);
        }
        
        
        protected override void Init()
        {
            if(InitValidate())
                return;

            base.Init();
        }


        protected override void Run()
        {



        }


        private void UpdateState(SceneState state)
        {
            switch (state)
            {
                case SceneState.None:

                    break;

                case SceneState.LoadIn:

                    
                    break;

                
                default:
                    Send($"{state}: State is not implemented!", true);
                    break;
            }
        }
    }
}
