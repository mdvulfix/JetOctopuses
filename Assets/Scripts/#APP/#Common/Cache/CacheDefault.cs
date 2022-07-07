using System;
using System.Collections.Generic;


namespace APP
{
    public class CacheDefault: ICache
    {        
        private bool m_Debug = true;
        
        private CacheConfig m_Config;
        private ICacheHandler m_Handler;

        protected readonly Dictionary<Type, ICacheable> m_Cache = new Dictionary<Type, ICacheable>(50);

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        

        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}

        

        public CacheDefault() { }
        public CacheDefault(IConfig config) =>
            Configure(config);

        
        // CONFIGURE //
        public void Configure(IConfig config)
        {
            if(IsConfigured)
                return;
            
            m_Config = (CacheConfig)config;
            
            m_Handler = m_Config.Handler;;
            

            OnConfigured();
        }
        
        public void Init()
        {   
            
            
            Subscribe();
            OnInitialized();
        }

        public void Dispose()
        {
            
            
            Unsubscribe();
            OnDisposed();
        }

        // SUBSCRUBE //
        public void Subscribe()
        {
            m_Handler.RecordToCahceRequired += Record;
            m_Handler.DeleteFromCacheRequired += Delete;
        }
        
        public void Unsubscribe()
        {
            m_Handler.RecordToCahceRequired -= Record;
            m_Handler.DeleteFromCacheRequired -= Delete;
        }
        
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
        
        // GET //
        public TCacheable Get<TCacheable>()
        where TCacheable: ICacheable
        {           
            var instance = default(TCacheable);
            
            if (m_Cache.TryGetValue(typeof(TCacheable), out var obj))
                instance = (TCacheable)obj;
            else
                Send($"{typeof(TCacheable).Name} was not found in cache!", LogFormat.Worning);

            return instance;
        }
             

        // RECORD //
        private void Record<TCacheable>(TCacheable instance)
        where TCacheable: ICacheable =>
            Record(instance);

        private void Record(ICacheable instance)
        {
            m_Cache.Add(instance.GetType(), instance);
            Send($"{instance.GetType().Name} was set to cache!");
        }
            
        // DELETE //
        private void Delete<TCacheable>(TCacheable instance)
        where TCacheable: ICacheable =>
            Delete(instance);

        private void Delete(ICacheable instance)
        {   
            if(Contains(instance))
            {
                m_Cache.Remove(instance.GetType());
                Send($"{instance.GetType().Name} removed from cache!");
            }
        }
 
        
        // HELPERS //
        private string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);


        // CALLBACK //           
        private void OnConfigured()
        {
            Send($"Configuration successfully completed!");
            IsConfigured = true;
            Configured?.Invoke();
        }

        private void OnInitialized()
        {
            Send($"Initialization successfully completed!");
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDisposed()
        {
            Send($"Dispose process successfully completed!");
            IsInitialized = false;
            Disposed?.Invoke();
        }
    }

    public struct CacheConfig : IConfig
    {
        public CacheConfig(ICacheHandler handler)
        {
            Handler = handler;
        }
        
        public ICacheHandler Handler { get; internal set; }
    }

    public interface ICache: IConfigurable, IInitializable, ISubscriber
    {
        bool Contains<TCacheable>() 
        where TCacheable: ICacheable;

        TCacheable Get<TCacheable>() 
        where TCacheable: ICacheable;
    }





}