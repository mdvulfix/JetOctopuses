using System;
using Core;
using Core.Factory;
using UnityEngine;

namespace Core.Pool
{
    [Serializable]
    public class PoolDefault : PoolModel, IPool
    {

        private Transform m_Parent;

        public PoolDefault() { }
        public PoolDefault(params object[] args)
            => Init(args);


        public bool Push<TPoolable>(TPoolable poolable)
        where TPoolable : IPoolable
          => base.Push(poolable);


        public bool Pop<TPoolable>(out TPoolable poolable)
        where TPoolable : IPoolable
        {
            if (base.Pop(out var instance))
            {
                poolable = (TPoolable)instance;
                return true;
            }
            else
            {
                poolable = default(TPoolable);
                return false;
            }
        }

        public bool Peek<TPoolable>(out TPoolable poolable)
        where TPoolable : IPoolable
        {
            if (base.Peek(out var instance))
            {
                poolable = (TPoolable)instance;
                return true;
            }
            else
            {
                poolable = default(TPoolable);
                return false;
            }
        }



        public override void Update() { }

    }

    public partial class PoolFactory : Factory<IPool>
    {
        private PoolConfig m_Config;

        private PoolDefault GetPoolDefault(params object[] args)
        {
            var instance = new PoolDefault();

            if (args.Length > 0)
                try { instance.Init((PoolConfig)args[(int)PoolModel.Params.Config]); }
                catch { Debug.Log("Custom factory not found! The instance will be created by default."); }


            return instance;
        }

    }

}