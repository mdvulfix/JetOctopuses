using System;

namespace APP
{
    public interface IInitializable
    {
        event Action Initialized;
        event Action Disposed;
        
        bool IsInitialized {get; }
    
        void Init();
        void Dispose();

        bool CheckInitialize();
    }
}