using UnityEngine;
using Core;
using Core.Factory;
using System;

namespace Core.Pool
{

   public class PoolController : ModelController, IPoolController
   {


      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;


      private IPool m_Pool;



      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected PoolControllerConfig m_Config;



      public event Action<bool> Configured;
      public event Action<bool> Initialized;



      public PoolController() { }
      public PoolController(params object[] args)
          => Configure(args);


      public enum Params
      {
         Config,
         Factory
      }


      // CONFIGURE //
      public override void Configure(params object[] args)
      {
         var config = (int)Params.Config;

         if (args.Length > 0)
         {
            try { m_Config = (PoolControllerConfig)args[config]; }
            catch { Debug.LogWarning($"{this.GetName()} config was not found. Configuration failed!"); return; }
         }

         m_Config = (PoolControllerConfig)args[config];


         var factoryPoolable = new FactoryDefault();
         //var limit = 5;
         //var poolConfig = new PoolConfig(limit, () => factoryPoolable.Get<TPoolable>());
         //m_Pool = new Pool<TPoolable>();
         //m_Pool.Configure(poolConfig);
         //m_Pool.Init();


         m_isConfigured = true;
         Configured?.Invoke(m_isConfigured);
         if (m_isDebug) Debug.Log($"{this.GetName()} configured.");
      }

      public override void Init()
      {



         m_isInitialized = true;
         Initialized?.Invoke(m_isInitialized);
         if (m_isDebug) Debug.Log($"{this.GetName()} initialized.");

      }

      public override void Dispose()
      {



         m_isInitialized = false;
         Initialized?.Invoke(m_isInitialized);
         if (m_isDebug) Debug.Log($"{this.GetName()} disposed.");
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
            poolable.Init();
            return true;
         }

         return false;
      }

      public bool Peek(out IPoolable poolable)
      {
         if (m_Pool.Peek(out poolable))
         {
            poolable.Init();
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

      public PoolControllerConfig(IPool pool)
      {
         Pool = pool;
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
