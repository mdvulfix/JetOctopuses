using System;
using UnityEngine;


namespace Core
{
    public abstract class ModelCommon
    {

        public event Action<bool> Configured;
        public event Action<bool> Initialized;

        // CONFIGURE //
        public virtual void Configure(params object[] args) => Configured?.Invoke(true);
        public virtual void Init() => Initialized?.Invoke(true);
        public virtual void Dispose() => Initialized?.Invoke(false);


    }

}

