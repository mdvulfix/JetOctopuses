using System;
using System.Collections.Generic;
using Core.Factory;
using UnityEngine;

namespace Core.Pool
{
   public abstract class PoolModel : ModelBasic
   {

      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;


      private Stack<IPoolable> m_Poolables;


      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected PoolConfig m_Config;


      public event Action<IResult> Configured;
      public event Action<IResult> Initialized;


      public enum Params
      {
         Config,
         Factory
      }

      // CONFIGURE //
      public override void Configure(params object[] args)
      {
         var config = (int)Params.Config;

         var result = default(IResult);
         var log = "...";


         if (args.Length > 0)
         {
            try { m_Config = (PoolConfig)args[config]; }
            catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
         }



         m_isConfigured = true;
         log = $"{this.GetName()} configured.";
         result = new Result(this, m_isConfigured, log, m_isDebug);
         Configured?.Invoke(result);
      }

      public override void Init()
      {
         var result = default(IResult);
         var log = "...";

         m_Poolables = new Stack<IPoolable>(10);


         m_isInitialized = true;
         log = $"{this.GetName()} initialized.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }

      public override void Dispose()
      {
         var result = default(IResult);
         var log = "...";

         m_Poolables.Clear();


         m_isInitialized = false;
         log = $"{this.GetName()} disposed.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }



      public bool Push(IPoolable poolable)
      {
         m_Poolables.Push(poolable);
         return true;
      }

      public bool Pop(out IPoolable poolable)
      {
         poolable = null;

         if (m_Poolables.Count > 0)
         {
            poolable = m_Poolables.Pop();
            return true;
         }

         return false;
      }

      public bool Peek(out IPoolable poolable)
      {
         poolable = null;

         if (m_Poolables.Count > 0)
         {
            poolable = m_Poolables.Peek();
            return true;
         }

         return false;
      }


      public abstract void Update();



      // FACTORY //
      public static TPool Get<TPool>(params object[] args)
      where TPool : IPool
      {
         IFactory factoryCustom = null;

         if (args.Length > 0)
         {
            try { factoryCustom = (IFactory)args[(int)Params.Factory]; }
            catch { Debug.LogWarning("Custom factory not found! The instance will be created by default."); }
         }

         var factory = (factoryCustom != null) ? factoryCustom : new PoolFactory();
         var instance = factory.Get<TPool>(args);

         return instance;
      }

   }




   public partial class PoolFactory : Factory<IPool>
   {
      public PoolFactory()
      {
         Set<PoolDefault>(Constructor.Get((args) => GetDefault(args)));

      }
   }






}

namespace Core
{
   public interface IPool : IConfigurable, IUpdatable
   {
      bool Push(IPoolable poolable);
      bool Pop(out IPoolable poolable);
      bool Peek(out IPoolable poolable);

   }

   public class PoolConfig : IConfig
   {

   }
   public interface IPoolable : IConfigurable
   {

   }
}