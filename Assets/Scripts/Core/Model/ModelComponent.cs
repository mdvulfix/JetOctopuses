using System;
using UnityEngine;


namespace Core
{
    public abstract class ModelComponent : MonoBehaviour
    {

        public event Action<bool> Configured;
        public event Action<bool> Initialized;
        public event Action<bool> Loaded;
        public event Action<bool> Activated;

        // CONFIGURE //
        public virtual void Configure(params object[] args) => Configured?.Invoke(true);
        public virtual void Init() => Initialized?.Invoke(true);
        public virtual void Dispose() => Initialized?.Invoke(false);

        // COMPONENT //
        public virtual void Load() => Loaded?.Invoke(true);
        public virtual void Activate() => Activated?.Invoke(true);
        public virtual void Deactivate() => Activated?.Invoke(true);
        public virtual void Unload() => Loaded?.Invoke(true);


        // UNITY //
        private void Awake() => Configure();
        private void OnEnable() => Init();
        private void OnDisable() => Dispose();

    }

    public interface IComponent
    {

    }
}

