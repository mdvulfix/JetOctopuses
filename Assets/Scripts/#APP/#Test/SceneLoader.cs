using UnityEngine;
using SERVICE.Handler;
using System;

namespace APP.Test
{
    public class SceneLoader : SceneObject, IInitializable
    {
        public bool IsInitialized => throw new NotImplementedException();

        public event Action Initialized;
        public event Action Disposed;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        private async void SceneActivate() 
        {
            await SceneHandler.Activate(SceneIndex.Net);  
        }


    }
}