using System;


namespace Core
{
    public interface IConfigurable : IDisposable
    {
        bool isInitialized { get; }

        event Action<IResult> Initialized;
        event Action<IResult> Disposed;

        void Init(params object[] args);
    }
}

