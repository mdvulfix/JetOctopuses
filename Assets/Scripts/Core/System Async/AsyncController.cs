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

      public event Action<bool> Configured;
      public event Action<bool> Initialized;
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

         if (args.Length > 0)
         {
            try { m_Config = (AsyncControllerConfig)args[config]; }
            catch { Debug.LogWarning($"{this.GetName()} config was not found. Configuration failed!"); return; }
         }

         m_Config = (AsyncControllerConfig)args[config];


         m_isConfigured = true;
         Configured?.Invoke(m_isConfigured);
         if (m_isDebug) Debug.Log($"{this.GetName()} configured.");
      }

      public override void Init()
      {
         if (m_AwaiterIsReady == null)
            m_AwaiterIsReady = new List<IAwaiter>(m_AwaiterIsReadyLimit);

         if (m_FuncExecuteQueue == null)
            m_FuncExecuteQueue = new List<FuncAsyncInfo>(100);


         m_FuncQueueAwaiter = AwaiterModel.Get<AwaiterDefault>();
         var awaiterConfig = new AwaiterConfig();

         m_FuncQueueAwaiter.Configure(awaiterConfig);
         m_FuncQueueAwaiter.Init();

         var factoryPoolable = new AwaiterFactory();
         var limit = 5;
         var poolConfig = new PoolConfig();
         var pool = new PoolDefault();
         pool.Configure(poolConfig);
         pool.Init();

         var poolControllerConfig = new PoolControllerConfig(pool);
         m_PoolController = new PoolController();
         m_PoolController.Configure(poolControllerConfig);
         m_PoolController.Init();


         m_isInitialized = true;
         Initialized?.Invoke(m_isInitialized);
         if (m_isDebug) Debug.Log($"{this.GetName()} initialized.");

      }

      public override void Dispose()
      {

         m_PoolController.Dispose();
         m_FuncQueueAwaiter.Dispose();


         m_isInitialized = false;
         Initialized?.Invoke(m_isInitialized);
         if (m_isDebug) Debug.Log($"{this.GetName()} disposed.");
      }



      public void Update()
      {
         m_PoolController.Update();

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
            if (m_PoolController.Pop(out var newAwaiter))
            {
               newAwaiter.Initialized += OnAwaiterInitialized;
               newAwaiter.Disposed += OnAwaiterDisposed;
               newAwaiter.FuncStarted += OnAwaiterFuncStarted;
               newAwaiter.FuncCompleted += OnAwaiterFuncComplete;

               newAwaiter.Init();
               awaiter = newAwaiter;
               return true;
            }
         }
         catch (Exception exception) { ($"Pop awaiter is failed! Exception: {exception.Message}").Send(LogFormat.Warning); }


          ($"Pop awaiter not found!").Send(LogFormat.Warning);

         return false;

      }

      private void PushAwaiter(IAwaiter awaiter)
      {

         awaiter.Initialized -= OnAwaiterInitialized;
         awaiter.Disposed -= OnAwaiterDisposed;
         awaiter.FuncStarted -= OnAwaiterFuncStarted;
         awaiter.FuncCompleted -= OnAwaiterFuncComplete;
         awaiter.Dispose();

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


      private void OnAwaiterInitialized(IAwaiter awaiter, bool status)
      {
         if (status)
            m_AwaiterIsReady.Add(awaiter);
      }

      private void OnAwaiterDisposed(IAwaiter awaiter, bool status)
      {
         if (!status && m_AwaiterIsReady.Contains(awaiter))
            m_AwaiterIsReady.Remove(awaiter);
      }

      private void OnAwaiterFuncStarted(IAwaiter awaiter)
      {
         if (m_AwaiterIsReady.Contains(awaiter))
            m_AwaiterIsReady.Remove(awaiter);
      }

      private void OnAwaiterFuncComplete(IAwaiter awaiter)
      {
         m_AwaiterIsReady.Add(awaiter);
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