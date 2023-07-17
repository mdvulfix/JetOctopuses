using UnityEngine;
using Core;
using Core.Factory;
using System;
using Core.Async;

namespace Core.Pool
{

    public class PoolController : ModelController, IPoolController
    {


        private PoolControllerConfig m_Config;
        private IPool m_Pool;
        private IFactory m_PoolableFactory;





        [Header("Stats")]
        [SerializeField] private bool m_isInitialized;


        [Header("Debug")]
        [SerializeField] protected bool m_isDebug = true;





        public event Action<IResult> Initialized;



        public PoolController() { }
        public PoolController(params object[] args)
            => Init(args);


        public enum Params
        {
            Config,
            Factory
        }



        // CONFIGURE //

        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            var result = default(IResult);
            var log = "...";

            if (args.Length > 0)
            {
                try { m_Config = (PoolControllerConfig)args[config]; }
                catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
            }

            m_Pool = m_Config.Pool;
            m_PoolableFactory = m_Config.PoolableFactory;

            var poolConfig = new PoolConfig(m_PoolableFactory);
            m_Pool.Init(poolConfig);


            m_isInitialized = true;
            log = $"{this.GetName()} initialized.";
            result = new Result(this, m_isInitialized, log, m_isDebug);
            Initialized?.Invoke(result);

        }

        public override void Dispose()
        {
            var result = default(IResult);
            var log = "...";

            m_Pool.Dispose();

            m_isInitialized = false;
            log = $"{this.GetName()} disposed.";
            result = new Result(this, m_isInitialized, log, m_isDebug);
            Initialized?.Invoke(result);

        }



        public void Push<TPoolable>(TPoolable poolable)
        where TPoolable : IPoolable
          => Push(poolable);

        public bool Pop<TPoolable>(out TPoolable poolable)
        where TPoolable : IPoolable
          => Pop(out poolable);

        public bool Peek<TPoolable>(out TPoolable poolable)
        where TPoolable : IPoolable
          => Peek(out poolable);


        public void Push(IPoolable poolable)
        {
            poolable.Dispose();
            m_Pool.Push(poolable);
        }

        public bool Pop(out IPoolable poolable)
        {
            if (m_Pool.Pop(out poolable))
            {
                return true;
            }

            return false;
        }

        public bool Peek(out IPoolable poolable)
        {
            if (m_Pool.Peek(out poolable))
            {
                return true;
            }

            return false;
        }



        public void Update()
        {
            m_Pool.Update();
        }
    }

    public class PoolControllerConfig : IConfig
    {
        public IPool Pool { get; private set; }
        public IFactory PoolableFactory { get; private set; }


        public PoolControllerConfig(IPool pool, IFactory poolableFactory)
        {
            Pool = pool;
            PoolableFactory = poolableFactory;
        }
    }

}


namespace Core
{
    public interface IPoolController : IController, IUpdatable
    {
        bool Peek(out IPoolable poolable);
        bool Pop(out IPoolable poolable);
        void Push(IPoolable poolable);
    }
}
