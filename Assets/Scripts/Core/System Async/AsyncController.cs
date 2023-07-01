using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Core.Pool;


namespace Core.Async
{
   public class AsyncController : ModelController, IAsyncController
   {

      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;


      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected AsyncControllerConfig m_Config;


      private static List<IAwaiter> m_AwaiterIsReady;
      private int m_AwaiterIsReadyLimit = 5;

      private static List<FuncAsyncInfo> m_FuncExecuteQueue;
      private IAwaiter m_FuncQueueAwaiter;

      private IPoolController m_PoolController;

      public event Action<IResult> Configured;
      public event Action<IResult> Initialized;
      public event Action<FuncAsyncInfo> FuncAsyncExecuted;

      public AsyncController() { }
      public AsyncController(params object[] args)
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

         var result = default(IResult);
         var log = "...";

         if (args.Length > 0)
         {
            try { m_Config = (AsyncControllerConfig)args[config]; }
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

         if (m_AwaiterIsReady == null)
            m_AwaiterIsReady = new List<IAwaiter>(m_AwaiterIsReadyLimit);

         if (m_FuncExecuteQueue == null)
            m_FuncExecuteQueue = new List<FuncAsyncInfo>(100);


         m_FuncQueueAwaiter = AwaiterModel.Get<AwaiterDefault>();
         var awaiterConfig = new AwaiterConfig();

         m_FuncQueueAwaiter.Configure(awaiterConfig);
         m_FuncQueueAwaiter.Init();

         var factoryPoolable = new AwaiterFactory();
         //var limit = 5;
         var poolConfig = new PoolConfig();
         var pool = new PoolDefault();
         pool.Configure(poolConfig);
         pool.Init();

         var poolControllerConfig = new PoolControllerConfig(pool);
         m_PoolController = new PoolController();
         m_PoolController.Configure(poolControllerConfig);
         m_PoolController.Init();



         m_isInitialized = true;
         log = $"{this.GetName()} initialized.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }

      public override void Dispose()
      {
         var result = default(IResult);
         var log = "...";



         m_PoolController.Dispose();
         m_FuncQueueAwaiter.Dispose();

         m_isInitialized = false;
         log = $"{this.GetName()} disposed.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }









      public void Update()
      {

         LimitUpdate();
         FuncQueueUpdate();
      }


      public void ExecuteAsync(Func<Action<bool>, IEnumerator> func)
      {
         if (GetAwaiter(out var awaiter))
         {
            if (awaiter.isReady == true)
            {
               //awaiter.Run(this, func);
               //FuncAsyncExecuted?.Invoke(new FuncAsyncInfo(awaiter, func));
               return;
            }

            m_FuncExecuteQueue.Add(new FuncAsyncInfo(awaiter, func));
         }
      }

      private bool GetAwaiter(out IAwaiter awaiter)
      {
         awaiter = null;

         if ((m_AwaiterIsReady.Count < m_AwaiterIsReadyLimit))
            LimitUpdate();

         awaiter = m_AwaiterIsReady[0];
         return true;
      }

      private bool PopAwaiter(out IAwaiter awaiter)
      {
         awaiter = null;

         try
         {
            if (m_PoolController.Pop(out var poolable))
            {
               if (poolable is IAwaiter)
               {
                  awaiter = (IAwaiter)poolable;
                  awaiter.Initialized += OnAwaiterInitialized;
                  awaiter.Ready += OnAwaiterReady;
                  awaiter.Init();
                  return true;

               }

               ($"Pop awaiter was not found!").Send(this, m_isDebug, LogFormat.Warning);
               return false;
            }
         }
         catch (Exception exception) { ($"Pop awaiter is failed! Exception: {exception.Message}").Send(LogFormat.Warning); }


          ($"Pop awaiter not found!").Send(LogFormat.Warning);

         return false;

      }

      private void PushAwaiter(IAwaiter awaiter)
      {
         awaiter.Dispose();
         awaiter.Initialized -= OnAwaiterInitialized;
         awaiter.Ready -= OnAwaiterReady;

         m_PoolController.Push(awaiter);
      }


      private void LimitUpdate()
      {
         // Check awaiters in ready state limit;
         var awaiterIsReadyNumber = m_AwaiterIsReady.Count;

         // If the limit is less than the current number of awaiters, push unnecessary awaiters in the pool
         if (awaiterIsReadyNumber > m_AwaiterIsReadyLimit)
         {
            var number = awaiterIsReadyNumber - m_AwaiterIsReadyLimit;
            for (int i = 0; i < number; i++)
               PushAwaiter(m_AwaiterIsReady[i]);
         }
         // else, pop awaiters from the pool in the number of missing up to the limit
         else
         {
            var number = m_AwaiterIsReadyLimit - awaiterIsReadyNumber;
            for (int i = 0; i < number; i++)
               PopAwaiter(out var awaiter);
         }
      }

      private void FuncQueueUpdate()
      {
         //if (m_FuncQueueAwaiter.isReady)
         //   m_FuncQueueAwaiter.Run(this, FuncQueueExecuteAsync);
      }

      private IEnumerator FuncQueueExecuteAsync(Action<bool> callback)
      {
         var funcsReadyToBeExecuted = (from FuncAsyncInfo funcInfo in m_FuncExecuteQueue
                                       where funcInfo.Awaiter.isReady == true
                                       select funcInfo).ToArray();


         if (funcsReadyToBeExecuted.Length > 0)
         {
            foreach (var info in funcsReadyToBeExecuted)
            {
               if (m_FuncExecuteQueue.Contains(info))
                  m_FuncExecuteQueue.Remove(info);

               //info.Awaiter.Run(this, info.FuncAsync);
               //FuncAsyncExecuted?.Invoke(info);
            }
         }

         yield return null;

         callback.Invoke(true);
      }


      private void OnAwaiterInitialized(IResult result)
      {
         if (result == null)
            return;

         var status = result.Status;
         var awaiter = result.Context is IAwaiter ? (IAwaiter)result.Context : null;

         switch (status)
         {
            case true:
               if (awaiter != null) m_AwaiterIsReady.Add(awaiter);
               break;

            case false:
               if (awaiter != null && m_AwaiterIsReady.Contains(awaiter)) m_AwaiterIsReady.Remove(awaiter);
               break;

         }

      }

      private void OnAwaiterReady(IResult result)
      {
         if (result == null)
            return;

         var status = result.Status;
         var awaiter = result.Context is IAwaiter ? (IAwaiter)result.Context : null;

         switch (status)
         {
            case true:
               if (awaiter != null) m_AwaiterIsReady.Add(awaiter);
               break;

            case false:
               if (awaiter != null && m_AwaiterIsReady.Contains(awaiter)) m_AwaiterIsReady.Remove(awaiter);
               break;

         }
      }
   }


   public interface IAsyncController : IController, IUpdatable
   {

   }



   public class AsyncControllerConfig : IConfig
   {

   }

   public struct FuncAsyncInfo
   {
      public IAwaiter Awaiter { get; private set; }
      public Func<Action<bool>, IEnumerator> FuncAsync { get; private set; }

      public FuncAsyncInfo(IAwaiter awaiter, Func<Action<bool>, IEnumerator> func)
      {
         FuncAsync = func;
         Awaiter = awaiter;
      }
   }
}