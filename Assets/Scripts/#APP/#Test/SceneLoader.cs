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

        public IMessage Dispose()
        {
            throw new NotImplementedException();
        }

        public IMessage Init()
        {
            throw new NotImplementedException();
        }
    }
}