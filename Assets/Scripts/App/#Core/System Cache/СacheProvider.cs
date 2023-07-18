
using System;

namespace Core.Cache
{

    public class СacheProvider<TCacheable>
    where TCacheable : ICacheable
    {

        public bool Contains()
        {
            using (var handler = new CacheHandler<TCacheable>(new CacheHandlerConfig()))
            {
                using (var cache = new Cache<TCacheable>(new CacheConfig(handler)))
                {
                    return cache.Contains();
                }
            }
        }


        public bool Get(out TCacheable cacheable)
        {
            cacheable = default(TCacheable);

            using (var handler = new CacheHandler<TCacheable>(new CacheHandlerConfig()))
            {
                using (var cache = new Cache<TCacheable>(new CacheConfig(handler)))
                {
                    return cache.Get(out cacheable);
                }

            }
        }
    }
}
