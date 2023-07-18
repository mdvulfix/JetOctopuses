using System;
using UnityEngine;

namespace Core.Cache
{


    public class CacheHandler<TCacheable> : CacheHandler
    where TCacheable : ICacheable
    {
        public CacheHandler() { }
        public CacheHandler(params object[] args)
            => Init(args);

        // RECORD //
        public void Record(TCacheable instance)
            => base.Record(instance);


        // CLEAR //          
        public void Clear(TCacheable instance)
            => base.Clear(instance);

    }

    public class CacheHandler : ModelHandler, ICacheHandler
    {
        private CacheHandlerConfig m_Config;
        private bool m_isDebug = true;


        public string Label => "CacheHandler";

        public event Action<ICacheable> RecordRequired;
        public event Action<ICacheable> ClearRequired;

        public CacheHandler() { }
        public CacheHandler(params object[] args)
            => Init(args);


        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (CacheHandlerConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {Label} config was not found. Configuration failed!"); return; }


            base.Init();
        }


        // RECORD //
        public void Record<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable =>
            Record(instance);

        public void Record(ICacheable instance)
            => RecordRequired?.Invoke(instance);

        // CLEAR //
        public void Clear<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable =>
            Clear(instance);

        public void Clear(ICacheable instance)
            => ClearRequired?.Invoke(instance);



    }

    public struct CacheHandlerConfig : IConfig
    {

    }

    public interface ICacheHandler : IConfigurable, IHandler
    {

        event Action<ICacheable> RecordRequired;
        event Action<ICacheable> ClearRequired;


        void Record<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable;

        void Record(ICacheable instance);


        void Clear<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable;

        void Clear(ICacheable instance);


    }

    public interface IHandler
    {

    }
}

