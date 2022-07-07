namespace APP
{
    public static class СacheProvider<TCacheable>
    where TCacheable: ICacheable
    {
        private static ICacheHandler m_Handler = 
            new CacheHandlerDefault<TCacheable>(new CacheHandlerConfig());

        public static bool Contains() =>     
            m_Handler.Cache.Contains<TCacheable>();
    
        public static TCacheable Get() =>     
            m_Handler.Cache.Get<TCacheable>();
    
    }
}