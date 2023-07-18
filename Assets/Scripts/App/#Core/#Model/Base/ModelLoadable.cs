using System;
using UnityEngine;
using UComponent = UnityEngine.Component;

namespace Core
{
    public abstract class ModelLoadable
    {
        [SerializeField] private bool m_isInitialized;
        [SerializeField] private bool m_isLoaded;
        [SerializeField] private bool m_isCached;
        [SerializeField] private bool m_isActivated;


        public string Name => this.GetName();
        public Type Type => this.GetType();

        public bool isInitialized => m_isInitialized;
        public bool isLoaded => m_isLoaded;
        public bool isCached => m_isCached;
        public bool isActivated => m_isActivated;

        public event Action<IResult> Initialized;
        public event Action<IResult> Disposed;

        public event Action<IResult> Loaded;
        public event Action<IResult> Unloaded;

        public event Action<IResult> Recorded;
        public event Action<IResult> Cleared;

        public event Action<IResult> Activated;
        public event Action<IResult> Deactivated;

        public enum Params
        {
            Config,
            Factory
        }


        // CONFIGURE //
        public virtual void Init(params object[] args)
            => OnInitComplete(new Result(this, true, $"{this.GetName()} initialized."));

        public virtual void Dispose()
            => OnDisposeComplete(new Result(this, true, $"{this.GetName()} disposed."));

        // LOAD //
        public virtual void Load()
            => OnDeactivateComplete(new Result(this, true, $"{this.GetName()} loaded."));

        public virtual void Unload()
            => OnUnloadComplete(new Result(this, true, $"{this.GetName()} unloaded."));

        // CACHE //
        public virtual void Record()
            => OnRecordComplete(new Result(this, true, $"{this.GetName()} recorded to cache."));

        public virtual void Clear()
            => OnClearComplete(new Result(this, true, $"{this.GetName()} cleared from cache."));

        // ACTIVATE //
        public virtual void Activate()
            => OnActivateComplete(new Result(this, true, $"{this.GetName()} activated."));

        public virtual void Deactivate()
            => OnDeactivateComplete(new Result(this, true, $"{this.GetName()} deactivated."));


        // CALLBACK //
        protected virtual void OnInitComplete(IResult result, bool isDebag = true)
        {
            m_isInitialized = true;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Initialized?.Invoke(result);

        }

        protected virtual void OnDisposeComplete(IResult result, bool isDebag = true)
        {
            m_isInitialized = false;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Disposed?.Invoke(result);

        }

        protected virtual void OnLoadComplete(IResult result, bool isDebag = true)
        {
            m_isLoaded = true;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Loaded?.Invoke(result);
        }

        protected virtual void OnUnloadComplete(IResult result, bool isDebag = true)
        {
            m_isLoaded = false;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Unloaded?.Invoke(result);
        }

        protected virtual void OnRecordComplete(IResult result, bool isDebag = true)
        {
            m_isCached = true;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Recorded?.Invoke(result);
        }

        protected virtual void OnClearComplete(IResult result, bool isDebag = true)
        {
            m_isCached = false;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Cleared?.Invoke(result);
        }

        protected virtual void OnActivateComplete(IResult result, bool isDebag = true)
        {
            m_isActivated = true;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Activated?.Invoke(result);

        }

        protected virtual void OnDeactivateComplete(IResult result, bool isDebag = true)
        {
            m_isActivated = false;

            if (isDebag)
                Debug.Log($"{this.GetName()}: {result.Log}");

            Deactivated?.Invoke(result);

        }


    }

}

