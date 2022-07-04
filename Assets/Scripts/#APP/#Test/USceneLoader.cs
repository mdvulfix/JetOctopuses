using UnityEngine;
using SERVICE.Handler;

namespace APP.Test
{
    public class USceneLoader : UComponent
    {
        
        public override void Init()
        {
            var info = new Instance(this);
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