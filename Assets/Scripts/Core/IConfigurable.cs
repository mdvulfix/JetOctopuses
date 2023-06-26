using System;


namespace Core
{
    public interface IConfigurable : IDisposable
    {

        event Action<bool> Configured;
        event Action<bool> Initialized;

        void Configure(params object[] args);
        void Init();
    }
}

