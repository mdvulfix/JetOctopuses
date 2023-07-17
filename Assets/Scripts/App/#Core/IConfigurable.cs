using System;


namespace Core
{
    public interface IConfigurable : IDisposable
    {
        event Action<IResult> Initialized;
        void Init(params object[] args);
    }
}

