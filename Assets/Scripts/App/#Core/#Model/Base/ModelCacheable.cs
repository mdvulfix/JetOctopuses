using System;
using UnityEngine;

namespace Core
{
    public abstract class ModelCacheable
    {

        [SerializeField] private bool m_isCached;
        [SerializeField] private bool m_isInitialized;


        public bool isInitialized => m_isInitialized;
        public bool isCached => m_isCached;

        public string Name => this.GetName();
        public Type Type => this.GetType();

        public event Action<IResult> Recorded;
        public event Action<IResult> Cleared;

        public event Action<IResult> Initialized;
        public event Action<IResult> Disposed;

        public enum Params
        {
            Config,
            Factory
        }


        // CACHE //
        public virtual void Record()
            => OnRecordComplete(new Result(this, true, $"{this.GetName()} recorded to cache."));

        public virtual void Clear()
            => OnClearComplete(new Result(this, true, $"{this.GetName()} cleared from cache."));

        // CONFIGURE //
        public virtual void Init(params object[] args)
            => OnInitComplete(new Result(this, true, $"{this.GetName()} initialized."));

        public virtual void Dispose()
            => OnDisposeComplete(new Result(this, true, $"{this.GetName()} disposed."));



        // CALLBACK //

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


    }
}

