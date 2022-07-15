using System;

namespace APP
{
    public class CacheHandler<TCacheable> : Handler, ICacheHandler
    where TCacheable: ICacheable
    {
        private CacheHandlerConfig m_Config;
        private ICacheable m_Cacheable;

        private ICache<TCacheable> m_Cache;

        public CacheHandler() { }
        public CacheHandler(IConfig config) => Configure(config);

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<ICacheable> RecordToCahceRequired;
        public event Action<ICacheable> DeleteFromCacheRequired;        
        
        // CONFIGURE //
        public virtual IMessage Configure(IConfig config = null, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);
            
            
            
            if (config != null)
            {
                m_Config = (CacheHandlerConfig) config;
                m_Cacheable = m_Config.Cacheable;
            }

            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is object)
                        Send("Param is not used", LogFormat.Worning);
                }
            }

            m_Cache = new Cache<TCacheable>();
            Send(m_Cache.Configure(new CacheConfig(this)));

            IsConfigured = true;
            Configured?.Invoke();
            
            return Send("Configuration completed!");
        }

        // INIT //
        public virtual IMessage Init()
        {
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);

            Subscribe();

            Send(m_Cache.Init());

            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {          
            
            Send(m_Cache.Dispose());

            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
        }
        

        // SUBSCRIBE //
        public virtual void Subscribe()
        {
            m_Cacheable.RecordToCahceRequired += OnCacheableRecordToCahceRequired;
            m_Cacheable.DeleteFromCahceRequired += OnCacheableDeleteFromCahceRequired;
        }

        public virtual void Unsubscribe()
        {
            m_Cacheable.RecordToCahceRequired -= OnCacheableRecordToCahceRequired;
            m_Cacheable.DeleteFromCahceRequired -= OnCacheableDeleteFromCahceRequired;
        } 

        // CALLBACK //
        protected void OnCacheableRecordToCahceRequired(ICacheable cacheable) =>
            RecordToCahceRequired?.Invoke(cacheable);

        protected void OnCacheableDeleteFromCahceRequired(ICacheable cacheable) =>
            DeleteFromCacheRequired?.Invoke(cacheable);
            
    }
    public struct CacheHandlerConfig : IConfig
    {
        public ICacheable Cacheable { get; private set; }

        public CacheHandlerConfig(ICacheable cacheable)
        {
            Cacheable = cacheable;
        }
    }

    public interface ICacheHandler : IConfigurable, IInitializable, ISubscriber, IMessager
    {

        event Action<ICacheable> RecordToCahceRequired;
        event Action<ICacheable> DeleteFromCacheRequired;

    }
}