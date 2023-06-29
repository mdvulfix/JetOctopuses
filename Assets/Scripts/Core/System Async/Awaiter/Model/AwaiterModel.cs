using System;
using System.Collections;
using UnityEngine;
using Core;
using Core.Factory;

namespace Core.Async
{
   public abstract class AwaiterModel : ModelComponent
   {
      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;
      [SerializeField] private bool m_isActivated;





      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected AwaiterConfig m_Config;


      public bool isActivated => m_isActivated;

      public event Action<IResult> Configured;
      public event Action<IResult> Initialized;

      public event Action<IResult> Activated;


      public static string PREFAB_Folder;

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
            try { m_Config = (AwaiterConfig)args[config]; }
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



         m_isInitialized = true;
         log = $"{this.GetName()} initialized.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }

      public override void Dispose()
      {
         var result = default(IResult);
         var log = "...";



         m_isInitialized = false;
         log = $"{this.GetName()} disposed.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }


      // ACTIVATE //
      public virtual void Activate()
      {
         var result = default(IResult);
         var log = "...";


         m_isActivated = true;
         log = $"{this.GetName()} activated.";
         result = new Result(this, m_isActivated, log, m_isDebug);
         Activated?.Invoke(result);

      }

      public virtual void Deactivate()
      {
         var result = default(IResult);
         var log = "...";


         m_isActivated = false;
         log = $"{this.GetName()} deactivated.";
         result = new Result(this, m_isActivated, log, m_isDebug);
         Activated?.Invoke(result);

      }





      public abstract IResult Run(object context, Func<bool> action);



      // FACTORY //
      public static TAwaiter Get<TAwaiter>(params object[] args)
      where TAwaiter : IAwaiter
      {
         IFactory factoryCustom = null;

         if (args.Length > 0)
            try { factoryCustom = (IFactory)args[(int)Params.Factory]; }
            catch { Debug.Log("Custom factory not found! The instance will be created by default."); }


         var factory = (factoryCustom != null) ? factoryCustom : new AwaiterFactory();
         var instance = factory.Get<TAwaiter>(args);

         return instance;
      }

   }

   public interface IAwaiter : IConfigurable, IActivable, IPoolable
   {
      bool isReady { get; }

      event Action<IResult> Ready;

      IResult Run(object context, Func<bool> action);

   }

   public class AwaiterConfig
   {

   }

   public partial class AwaiterFactory : Factory<IAwaiter>
   {
      public AwaiterFactory()
      {
         Set<AwaiterDefault>(Constructor.Get((args) => GetDefault(args)));
      }

   }


}
