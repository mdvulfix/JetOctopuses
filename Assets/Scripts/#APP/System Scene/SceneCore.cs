using System;
using UnityEngine;
using APP;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneCore : SceneModel<SceneCore>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;
        [SerializeField] private ScreenSplash m_Splash;

        
        public override void Init()
        {
            var instance =  new Instance(this);
            var screens = new IScreen[2] 
            {
                m_Loading = Set<ScreenLoading>("Screen: Loading"),
                m_Splash = Set<ScreenSplash>("Screen: Splash")
            };

            var config =  new SceneConfig(instance, screens);
            
            Configure(config);
            base.Init();
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
                    Send($"{state}: State is not implemented!", LogFormat.Worning);
                    break;
            }
        }
    }
}
