using System;
using UnityEngine;
using UComponent = UnityEngine.Component;

namespace Core
{
    public abstract class ModelConfigurable
    {
        [SerializeField] private bool m_isInitialized;


        public bool isInitialized => m_isInitialized;

        public string Name => this.GetName();
        public Type Type => this.GetType();


        public event Action<IResult> Initialized;
        public event Action<IResult> Disposed;



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


    }

}

