using System;


namespace APP
{

    public class CacheHandlerDefault<TCacheable>: Handler, ICacheHandler
    where TCacheable: ICacheable
    {
        private CacheHandlerConfig m_Config;
        private ICacheable m_Cacheable;

        private static ICache m_Cache = new CacheDefault();
        
        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}

        public ICache Cache {get => m_Cache; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<ICacheable> RecordToCahceRequired;
        public event Action<ICacheable> DeleteFromCacheRequired;
        

        public CacheHandlerDefault() { }
        public CacheHandlerDefault(IConfig config) =>
            Configure(config);
        
        public void Configure(IConfig config)
        {
            m_Config = (CacheHandlerConfig)config;
            
            m_Cacheable = m_Config.Instance;
            m_Cache.Configure(new CacheConfig(this));
        }
        
        public void Init()
        {
            Subscribe();
            
            m_Cache.Init();

        }

        public void Dispose()
        {
            m_Cache.Dispose();
            
            Subscribe();
        }


        public void Subscribe()
        {
            m_Cacheable.Initialized += OnCacheableInitialized;
            m_Cacheable.Disposed += OnCacheableDispose;
        }

        public void Unsubscribe()
        {
            m_Cacheable.Initialized -= OnCacheableInitialized;
            m_Cacheable.Disposed -= OnCacheableDispose;
        }


        // CALLBACK //
        private void OnCacheableInitialized() =>
            RecordToCahceRequired?.Invoke(m_Cacheable);

        private void OnCacheableDispose() =>
            DeleteFromCacheRequired?.Invoke(m_Cacheable);
            
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

    public struct CacheHandlerConfig: IConfig
    {
        public ICacheable Instance {get; private set; }

        public CacheHandlerConfig(ICacheable instance)
        {
            Instance = instance;
        }
    }

    public interface ICacheHandler: IConfigurable, IInitializable, ISubscriber
    {
        ICache Cache {get; }
        
        event Action<ICacheable> RecordToCahceRequired;
        event Action<ICacheable> DeleteFromCacheRequired;

    }
}