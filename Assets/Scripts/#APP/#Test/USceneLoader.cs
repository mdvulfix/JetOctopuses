using UnityEngine;
using SERVICE.Handler;

namespace APP.Test
{
    public class USceneLoader : UComponent
    {
        
        protected override void Init()
        {
            var info = new InstanceInfo(this);
            var config = new Config(info);
            
            base.Configure(config);
            base.Init();

            SceneActivate();
        
        }
        
        
        private async void SceneActivate() 
        {
            await USceneHandler.Activate(SceneIndex.Net);  
        }


    }
}