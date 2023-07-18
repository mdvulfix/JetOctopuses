using System;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Core.Cache
{
    public abstract class CacheModel : ModelConfigurable, IObservable
    {
        private CacheConfig m_Config;
        private bool m_isDebug = true;

        private Dictionary<Type, ICacheable> m_Cache;

        public string Label => "Cache";

        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (CacheConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {Label} config was not found. Configuration failed!"); return; }


            m_Cache = new Dictionary<Type, ICacheable>(50);

            base.Init();
        }


        // OBSERVER //
        public void SetObserver(params object[] observers)
        {
            foreach (var observer in observers)
            {
                if (observer is ICacheHandler)
                {
                    var cacheHandler = (ICacheHandler)observer;
                    cacheHandler.RecordRequired += Record;
                    cacheHandler.ClearRequired += Clear;
                }
            }
        }

        public void RemoveObserver(params object[] observers)
        {
            foreach (var observer in observers)
            {
                if (observer is ICacheHandler)
                {
                    var cacheHandler = (ICacheHandler)observer;
                    cacheHandler.RecordRequired -= Record;
                    cacheHandler.ClearRequired -= Clear;
                }
            }
        }


        // CONTAINS //
        public bool Contains(Type key)
        {
            if (m_Cache.ContainsKey(key))
                return true;

            Debug.LogWarning($"{key} not found in cache!");
            return false;
        }

        // GET //
        public bool Get(Type key, out ICacheable instance)
        {
            if (m_Cache.TryGetValue(key, out instance))
                return true;

            Debug.LogWarning($"{key} not found in cache!");
            return false;
        }


        // RECORD //
        protected void Record(ICacheable instance)
        {
            m_Cache.Add(instance.GetType(), instance);
            Debug.Log($"{instance.GetName()} was recorded to cache!");
        }

        // CLEAR //
        protected void Clear(ICacheable instance)
        {
            if (m_Cache.ContainsKey(instance.GetType()))
            {
                m_Cache.Remove(instance.GetType());
                Debug.Log($"{instance.GetName()} cleared from cache!");
            }
        }



    }



    public struct CacheConfig : IConfig
    {
        public ICacheHandler Handler { get; private set; }

        public CacheConfig(ICacheHandler handler)
        {
            Handler = handler;
        }


    }


    public interface ICache : IConfigurable, IObservable
    {

        bool Contains(Type key);
        bool Get(Type key, out ICacheable instance);


    }

}

