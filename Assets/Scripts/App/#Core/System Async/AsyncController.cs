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
        [SerializeField] private bool m_isInitialized;


        private static Transform m_AsyncParent;
        private static Transform m_AsyncPool;


        private static Stack<IAwaiter> m_AwaiterIsReady;
        private int m_AwaiterIsReadyLimit = 5;

        private static Stack<FuncAsyncInfo> m_FuncExecuteQueue;
        private IAwaiter m_FuncQueueAwaiter;

        private IPoolController m_PoolController;
        private IPool m_Pool;
        private IFactory m_PoolableFactory;


        [Header("Debug")]
        [SerializeField] protected bool m_isDebug = true;

        [Header("Config")]
        [SerializeField] protected AsyncControllerConfig m_Config;



        public event Action<IResult> Initialized;
        public event Action<FuncAsyncInfo> FuncAsyncExecuted;

        public AsyncController() { }
        public AsyncController(params object[] args)
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
                try { m_Config = (AsyncControllerConfig)args[config]; }
                catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
            }


            if (m_AsyncParent == null)
                m_AsyncParent = new GameObject("Async").transform;

            if (m_AsyncPool == null)
            {
                m_AsyncPool = new GameObject("Pool").transform;
                m_AsyncPool.SetParent(m_AsyncParent);
            }



            if (m_AwaiterIsReady == null)
                m_AwaiterIsReady = new Stack<IAwaiter>(m_AwaiterIsReadyLimit);

            if (m_FuncExecuteQueue == null)
                m_FuncExecuteQueue = new Stack<FuncAsyncInfo>(100);

            /*
            for (int i = 0; i < m_AwaiterIsReadyLimit; i++)
            {
                m_FuncQueueAwaiter = AwaiterModel.Get<AwaiterDefault>();
                var awaiterConfig = new AwaiterConfig();

                m_FuncQueueAwaiter.SetParent(m_Async);
                m_FuncQueueAwaiter.Configure(awaiterConfig);
                m_FuncQueueAwaiter.Init();
                m_FuncQueueAwaiter.Activate();

            }

            */


            m_Pool = new PoolDefault();
            m_PoolableFactory = new AwaiterFactory();
            m_PoolController = new PoolController();
            m_PoolController.Init(new PoolControllerConfig(m_Pool, m_PoolableFactory));



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
            //LimitUpdate();
            //FuncQueueUpdate();
        }


        public IResult RunAsync(Func<bool> func)
        {
            var result = default(IResult);
            var log = "...";

            if (GetAwaiter(out var awaiter))
            {
                if (!awaiter.isReady)
                {
                    //m_FuncExecuteQueue.Add(new FuncAsyncInfo(awaiter, func));  
                    //FuncAsyncExecuted?.Invoke(new FuncAsyncInfo(awaiter, func));
                    log = "Awaiter was not found!";
                    result = new Result(this, false, log, m_isDebug, LogFormat.Warning);
                    return result;
                }

                return awaiter.Run(this, func);

            }

            log = "Awaiter was not found!";
            result = new Result(this, false, log, m_isDebug, LogFormat.Warning);
            return result;
        }

        private bool GetAwaiter(out IAwaiter awaiter)
        {
            awaiter = null;

            if ((m_AwaiterIsReady.Count < m_AwaiterIsReadyLimit))
                LimitUpdate();

            awaiter = m_AwaiterIsReady.Pop();
            return true;
        }

        private void LimitUpdate()
        {

            // If the limit is less than the current number of awaiters, push unnecessary awaiters in the pool
            if (m_AwaiterIsReady.Count > m_AwaiterIsReadyLimit)
            {
                for (int i = 0; i < m_AwaiterIsReady.Count - m_AwaiterIsReadyLimit; i++)
                {
                    var awaiter = m_AwaiterIsReady.Pop();

                    awaiter.Ready -= OnAwaiterReady;
                    awaiter.Deactivate();
                    awaiter.SetParent(m_AsyncPool);

                    //Push();
                }

            }
            // else, pop awaiters from the pool in the number of missing up to the limit
            else
            {
                for (int i = 0; i < m_AwaiterIsReadyLimit - m_AwaiterIsReady.Count; i++)
                {

                    var awaiter = m_PoolableFactory.Get<AwaiterDefault>(new AwaiterConfig());
                    awaiter.Ready += OnAwaiterReady;
                    awaiter.SetParent(m_AsyncParent);
                    awaiter.Activate();

                }

                //Pop(out var awaiter);
            }
        }







        private bool Pop(out IAwaiter awaiter)
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
                        awaiter.SetParent(m_AsyncParent);
                        awaiter.Init();
                        awaiter.Activate();
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

        private void Push(IAwaiter awaiter)
        {
            awaiter.Dispose();
            awaiter.SetParent(m_AsyncPool);
            awaiter.Initialized -= OnAwaiterInitialized;
            awaiter.Ready -= OnAwaiterReady;

            m_PoolController.Push(awaiter);
        }




        private void FuncQueueUpdate()
        {
            //if (m_FuncQueueAwaiter.isReady)
            //   m_FuncQueueAwaiter.Run(this, FuncQueueExecuteAsync);
        }
        /*
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

        */
        private void OnAwaiterInitialized(IResult result)
        {
            if (result == null)
                return;

            var status = result.Status;
            var awaiter = result.Context is IAwaiter ? (IAwaiter)result.Context : null;

            switch (status)
            {
                case true:
                    if (awaiter != null) m_AwaiterIsReady.Push(awaiter);
                    break;

                case false:
                    if (awaiter != null && m_AwaiterIsReady.Contains(awaiter))
                        m_AwaiterIsReady.Pop();
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
                    if (awaiter != null) m_AwaiterIsReady.Push(awaiter);
                    break;

                case false:
                    if (awaiter != null && m_AwaiterIsReady.Contains(awaiter))
                        m_AwaiterIsReady.Pop();
                    break;

            }
        }
    }


    public interface IAsyncController : IController, IUpdatable
    {
        IResult RunAsync(Func<bool> func);
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