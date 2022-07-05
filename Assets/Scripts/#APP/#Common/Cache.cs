using System;
using System.Collections.Generic;


namespace APP
{
    public class Cache<TCacheable>: Cache
    where TCacheable: ICacheable
    {
               
        public void OnCacheWrite(ICacheable instance) =>
            Set(instance);

        public void OnCacheClear(ICacheable instance) =>
            Remove(instance);
    }

    public class Cache: ICache
    {
        private bool m_Debug = true;
        protected readonly Dictionary<Type, ICacheable> m_Cache = new Dictionary<Type, ICacheable>(50);
        
        // CONTAINS //
        public bool Contains<TCacheable>()
        where TCacheable: ICacheable
        {            
            if(m_Cache.ContainsKey(typeof(TCacheable)))
                return true;

            Send($"{typeof(TCacheable).Name} not found in cache!");
            return false;
        }
         
        public bool Contains(ICacheable instance)
        {
            if(m_Cache.ContainsKey(instance.GetType()))
                return true;

            Send($"{instance.GetType().Name} not found in cache!");
            return false;
        }
        
        // SET //
        protected void Set<TCacheable>(TCacheable instance)
        where TCacheable: ICacheable =>
            Set(instance);

        private void Set(ICacheable instance)
        {
            m_Cache.Add(instance.GetType(), instance);
            Send($"{instance.GetType().Name} was set to cache!");
        }
            
        // REMOVE //
        protected void Remove<TCacheable>(TCacheable instance)
        where TCacheable: ICacheable =>
            Remove(instance);

        private void Remove(ICacheable instance)
        {   
            if(Contains(instance))
            {
                m_Cache.Remove(instance.GetType());
                Send($"{instance.GetType().Name} removed from cache!");
            }
        }

        // GET //
        public bool Get<TCacheable>(out TCacheable instance)
        where TCacheable: ICacheable
        {           
            instance = default(TCacheable);
            
            if (m_Cache.TryGetValue(typeof(TCacheable), out var obj))
            {
                instance = (TCacheable)obj;
                return true;
            }

            return false;
        }
              
        
        // HELPERS //
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);
    }

    public interface ICache
    {
        bool Contains<TCacheable>() where TCacheable: ICacheable;
        bool Get<TCacheable>(out TCacheable instance) where TCacheable: ICacheable;
    }

    public interface ICacheable
    {
        
        event Action<ICacheable> CacheWrite;
        event Action<ICacheable> CacheClear;
        
        void Subscrube();
        void Unsubscrube();

    }
}