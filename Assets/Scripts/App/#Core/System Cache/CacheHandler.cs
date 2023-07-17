using System;
/*
namespace Core.Cache
{
    
    
    public class CacheHandler<TCacheable> : ModelHandler, ICacheHandler
    where TCacheable : ICacheable
    {
        private CacheHandlerConfig m_Config;
        private ICacheable m_Cacheable;

        private ICache<TCacheable> m_Cache;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<ICacheable> RecordRequired;
        public event Action<ICacheable> DeleteRequired;

        public CacheHandler(params object[] param) => Configure(param);
        public CacheHandler() { }

        // CONFIGURE //
        public virtual void Configure(params object[] param)
        {
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Warning);
                return;
            }

            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (CacheHandlerConfig)obj;
                        m_Cacheable = m_Config.Cacheable;
                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Warning);
            }

            m_Cache = new Cache<TCacheable>();

            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }

        public virtual void Init()
        {
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Warning);
                return;
            }

            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Warning);
                return;
            }

            Subscribe();

            m_Cache.Configure(new CacheConfig(this));
            m_Cache.Init();

            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {

            m_Cache.Dispose();

            Unsubscribe();

            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }


        // SUBSCRIBE //
        public virtual void Subscribe()
        {
            m_Cacheable.RecordRequired += OnCacheableRecordToCahceRequired;
            m_Cacheable.DeleteRequired += OnCacheableDeleteFromCahceRequired;
        }

        public virtual void Unsubscribe()
        {
            m_Cacheable.RecordRequired -= OnCacheableRecordToCahceRequired;
            m_Cacheable.DeleteRequired -= OnCacheableDeleteFromCahceRequired;
        }

        // CALLBACK //
        protected void OnCacheableRecordToCahceRequired() =>
            RecordRequired?.Invoke(m_Cacheable);

        protected void OnCacheableDeleteFromCahceRequired() =>
            DeleteRequired?.Invoke(m_Cacheable);

    }
    public struct CacheHandlerConfig : IConfig
    {
        public ICacheable Cacheable { get; private set; }

        public CacheHandlerConfig(ICacheable cacheable)
        {
            Cacheable = cacheable;
        }
    }

    public interface ICacheHandler : IConfigurable, ISubscriber, IMessager
    {

        event Action<ICacheable> RecordRequired;
        event Action<ICacheable> DeleteRequired;

    }
}

*/