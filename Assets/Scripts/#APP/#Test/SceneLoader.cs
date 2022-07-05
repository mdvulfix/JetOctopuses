using UnityEngine;
using SERVICE.Handler;

namespace APP.Test
{
    public class SceneLoader : SceneObject
    {
        
        protected override void Init()
        {
            SceneActivate();
        }

        protected override void Dispose()
        {

        }


        private async void SceneActivate() 
        {
            await SceneHandler.Activate(SceneIndex.Net);  
        }


    }
}