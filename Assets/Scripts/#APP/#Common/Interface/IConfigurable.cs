using System;

namespace APP
{
    public interface IConfigurable
    {
        event Action Configured;
        event Action Initialized;
        event Action Disposed;

        bool IsConfigured {get; } 
        bool IsInitialized {get; }
        
        void Configure(params object[] param);
        void Init();
        void Dispose();

    }

}