using UnityEngine;

namespace Core.Cache
{
    public class Cache<TCacheable> : CacheDefault
    where TCacheable : ICacheable
    {
        public Cache() { }
        public Cache(params object[] args)
            => Init(args);


        // CONTAINS //
        public bool Contains()
            => base.Contains<TCacheable>();

        // GET //
        public bool Get(out TCacheable instance)
            => base.Get<TCacheable>(out instance);


        // RECORD //
        protected void Record(TCacheable instance)
            => base.Record<TCacheable>(instance);

        // CLEAR //
        protected void Clear(TCacheable instance)
            => base.Clear<TCacheable>(instance);


    }



    public class CacheDefault : CacheModel, ICache
    {
        public CacheDefault() { }
        public CacheDefault(params object[] args)
            => Init(args);


        // CONTAINS //
        public bool Contains<TCacheable>()
        where TCacheable : ICacheable
            => base.Contains(typeof(TCacheable));


        // GET //
        public bool Get<TCacheable>(out TCacheable instance)
        where TCacheable : ICacheable
        {
            if (base.Get(typeof(TCacheable), out var cacheable))
            {
                instance = (TCacheable)cacheable;
                return true;
            }

            instance = default(TCacheable);
            return false;
        }


        // RECORD //
        protected void Record<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable
            => base.Record(instance);

        // CLEAR //
        protected void Clear<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable
            => base.Clear(instance);


    }













}

