using System;

namespace APP
{
    public interface IConfigurable: IDisposable
    {
        event Action Configured;
        event Action Initialized;
        event Action Disposed;

        bool IsConfigured {get; } 
        bool IsInitialized {get; }
        
        void Configure(params object[] args);
        void Init();

    }

}