using UnityEngine;
using Core;
using Core.Factory;

namespace Core.Pool
{

   public class PoolController<TPoolable> : PoolControllerModel, IPoolController
   where TPoolable : IPoolable
   {

      private IPool m_Pool;


      public PoolController() { }
      public PoolController(params object[] args)
      {
         Configure(args);
         Init();
      }

      public override void Configure(params object[] args)
      {
         if (args.Length > 0)
         {
            base.Configure(args);
            return;
         }

         // DEFAULT CONFIG //
         Debug.LogWarning($"{this.GetName()} will be configured by default!");

         var factoryPoolable = new FactoryDefault();
         //var limit = 5;
         //var poolConfig = new PoolConfig(limit, () => factoryPoolable.Get<TPoolable>());
         //m_Pool = new Pool<TPoolable>();
         //m_Pool.Configure(poolConfig);
         //m_Pool.Init();

         var config = new PoolControllerConfig(m_Pool);
         base.Configure(config);

      }

      public override void Init()
      {
         base.Init();
      }

      public override void Dispose()
      {
         base.Dispose();
      }



      public void Push(TPoolable poolable)
        => Push(poolable);

      public bool Pop(out TPoolable poolable)
        => Pop(out poolable);

      public bool Peek(out TPoolable poolable)
        => Peek(out poolable);















   }
}

