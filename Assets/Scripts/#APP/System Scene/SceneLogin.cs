using System;
using UnityEngine;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public class SceneLogin : SceneModel<SceneLogin>, IScene
    {
        [SerializeField] private ScreenLoading m_Loading;

        private readonly string m_Name = "Scene: Login";

        public SceneLogin() => 
            Configure();

        public override void Configure()
        {
            var screens = new IScreen[1] 
            {
                m_Loading = SetComponent<ScreenLoading>("Screen: Loading"),
            };
            
            var config =  new SceneConfig(m_Name, this, screens);            
            base.Configure(config);
        }
    }
}