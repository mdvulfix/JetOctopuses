using APP;

namespace SERVICE.Handler
{
    public static class CacheHandler
    {
        public static bool Contains<TCacheable>(ICache cache) 
        where TCacheable: ICacheable
        {           
            if(cache.Get<TCacheable>(out var instance))
                return true;

            return false;
        }

        public static TCacheable Get<TCacheable>(ICache cache) 
        where TCacheable: ICacheable
        {           
            var instance = default(TCacheable);
            
            if(cache.Contains<TCacheable>())
                cache.Get<TCacheable>(out instance);

            return instance;
        }

    }
}