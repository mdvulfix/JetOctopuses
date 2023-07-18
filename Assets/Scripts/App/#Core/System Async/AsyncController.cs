using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;
using Core.Pool;
using Core.Factory;
//using Test;

namespace Core.Async
{
    public class AsyncController : ModelConfigurable, IAsyncController
    {

        private bool m_isDebug = true;

        private AsyncControllerConfig m_Config;

        [SerializeField] private static GameObject m_ObjAsync;
        [SerializeField] private static GameObject m_ObjAsyncPool;

        [SerializeField] private IFactory m_AwaiterFactory;

        private static List<IAwaiter> m_AwaiterIsReady;
        private int m_AwaiterIsReadyLimit = 1;

        private static Stack<IAsyncInfo> m_FuncExecuteQueue;
        private static Stack<IYield> m_FuncAwaiteQueue;
        private IAwaiter m_AwaiterYield;

        private IPoolController m_PoolController;

        public string Label => "AsyncController";

        public event Action<IAsyncInfo> FuncExecuted;

        public AsyncController() { }
        public AsyncController(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (AsyncControllerConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: config was not found. Configuration failed!"); return; }

            m_ObjAsync = m_Config.AsyncHolder;


            if (m_ObjAsync == null)
                m_ObjAsync = new GameObject("Async");


            if (m_ObjAsyncPool == null)
                m_ObjAsyncPool = new GameObject("Pool");

            m_ObjAsyncPool.transform.SetParent(m_ObjAsync.transform);

            if (m_AwaiterIsReady == null)
                m_AwaiterIsReady = new List<IAwaiter>(m_AwaiterIsReadyLimit);

            if (m_FuncExecuteQueue == null)
                m_FuncExecuteQueue = new Stack<IAsyncInfo>(100);

            if (m_FuncAwaiteQueue == null)
                m_FuncAwaiteQueue = new Stack<IYield>(100);

            // SET AWAITER //
            if (m_AwaiterFactory == null)
                m_AwaiterFactory = new AwaiterFactory();

            m_AwaiterYield = m_AwaiterFactory.Get<AwaiterDefault>();
            m_AwaiterYield.FuncExecuted += OnAwaiterYieldFuncExecutedRunNext;
            m_AwaiterYield.Init(new AwaiterConfig("AwaiterYield", m_ObjAsync));
            m_AwaiterYield.Activate();



            // SET POOL //
            m_PoolController = new PoolController(new PoolControllerConfig(m_ObjAsyncPool));


            AwaiterLimitUpdate();
            OnInitComplete(new Result(this, true, $"{Label} initialized."), m_isDebug);
        }

        public override void Dispose()
        {
            m_AwaiterYield.Deactivate();
            m_AwaiterYield.Dispose();
            m_AwaiterYield.FuncExecuted -= OnAwaiterYieldFuncExecutedRunNext;

            m_PoolController.Dispose();
            OnDisposeComplete(new Result(this, true, $"{Label} disposed."), m_isDebug);

        }

        public virtual void Update()
        {
            FuncQueueExecute();
        }


        public void Awaite(Func<bool> func)
            => m_FuncAwaiteQueue.Push(new WaitForFunc(func));

        public void Awaite(Action func)
            => m_FuncAwaiteQueue.Push(new WaitForFunc(func));

        public void Awaite(float delay)
            => m_FuncAwaiteQueue.Push(new WaitForTime(delay));




        public void Run()
        {
            if (m_FuncAwaiteQueue?.Count == 0)
                return;

            if (m_AwaiterYield.IsReady == true)
            {
                if (m_FuncAwaiteQueue.Count > 0)
                    m_AwaiterYield.Run(m_FuncAwaiteQueue.Pop());
            }
            else
            {
                Debug.LogWarning($"{this}: awaiter is not found!");
            }

        }

        public IEnumerator ExecuteAsync(IYield func)
        {
            if (GetAwaiter(out var awaiter))
            {
                yield return awaiter.ExecuteAsync(func, (result) => Debug.Log(result.Log));
            }
            else
            {
                Debug.LogWarning($"{this}: awaiter is not found!");
                m_FuncExecuteQueue.Push(new FuncAsyncInfo(func));
            }
        }




        private bool GetAwaiter(out IAwaiter awaiter)
        {
            awaiter = null;

            if ((m_AwaiterIsReady.Count < m_AwaiterIsReadyLimit))
                AwaiterLimitUpdate();

            if ((m_AwaiterIsReady.Count > 0))
            {
                awaiter = m_AwaiterIsReady.First();
                return true;
            }

            return false;
        }

        private void AwaiterLimitUpdate()
        {
            IAwaiter awaiter;

            // Check awaiters in ready state limit;
            var awaiterIsReadyNumber = m_AwaiterIsReady.Count;

            // If the limit is less than the current number of awaiters, push unnecessary awaiters in the pool
            if (awaiterIsReadyNumber > m_AwaiterIsReadyLimit)
            {
                var number = awaiterIsReadyNumber - m_AwaiterIsReadyLimit;
                for (int i = 0; i < number; i++)
                {
                    awaiter = m_AwaiterIsReady.First();
                    awaiter.Deactivate();
                    awaiter.Dispose();
                    awaiter.FuncInvoked -= OnAwaiterFuncInvoked;
                    awaiter.FuncExecuted -= OnAwaiterFuncExecuted;
                    awaiter.Initialized -= OnAwaiterInitialized;
                    awaiter.Disposed -= OnAwaiterDisposed;

                    m_PoolController.Push(awaiter);
                }
            }
            // else, pop awaiters from the pool in the number of missing up to the limit
            else
            {
                var number = m_AwaiterIsReadyLimit - awaiterIsReadyNumber;
                for (int i = 0; i < number; i++)
                {
                    if (!m_PoolController.Pop(out awaiter))
                        awaiter = m_AwaiterFactory.Get<AwaiterDefault>();

                    awaiter.Initialized += OnAwaiterInitialized;
                    awaiter.Disposed += OnAwaiterDisposed;
                    awaiter.FuncInvoked += OnAwaiterFuncInvoked;
                    awaiter.FuncExecuted += OnAwaiterFuncExecuted;
                    awaiter.Init(new AwaiterConfig("Awaiter", m_ObjAsync));
                    awaiter.Activate();
                }
            }
        }

        private void FuncQueueExecute()
        {
            if (m_FuncExecuteQueue?.Count == 0)
                return;

            var awaiterIsReadyArr = (from IAwaiter awaiter in m_AwaiterIsReady
                                     where awaiter.IsReady == true
                                     select awaiter).ToArray();

            if (awaiterIsReadyArr.Length > 0)
            {
                if (m_FuncExecuteQueue.Count > 0)
                {
                    var info = m_FuncExecuteQueue.Pop();
                    info.Awaiter = awaiterIsReadyArr.First();
                    info.Awaiter.ExecuteAsync(info.Func, (result) => Debug.Log(result.Log));
                }
            }
        }


        private void OnAwaiterInitialized(IResult result)
        {
            if (!result.State)
                return;

            var awaiter = result.Context.Convert<IAwaiter>();
            m_AwaiterIsReady.Add(awaiter);

        }

        private void OnAwaiterDisposed(IResult result)
        {
            if (!result.State)
                return;

            var awaiter = result.Context.Convert<IAwaiter>();

            if (m_AwaiterIsReady.Contains(awaiter))
                m_AwaiterIsReady.Remove(awaiter);

        }


        private void OnAwaiterFuncInvoked(IAwaiter awaiter)
        {
            if (m_AwaiterIsReady.Contains(awaiter))
                m_AwaiterIsReady.Remove(awaiter);

            AwaiterLimitUpdate();

        }

        private void OnAwaiterFuncExecuted(IAwaiter awaiter)
        {
            m_AwaiterIsReady.Add(awaiter);

            AwaiterLimitUpdate();
        }


        private void OnAwaiterYieldFuncExecutedRunNext(IAwaiter awaiter)
        {
            if (m_FuncAwaiteQueue?.Count == 0)
                return;

            if (awaiter.IsReady == true)
            {
                if (m_FuncAwaiteQueue.Count > 0)
                    awaiter.Run(m_FuncAwaiteQueue.Pop());
            }
        }


        private void OnAwaiterReadyChanged(IResult result)
        {
            IAwaiter awaiter = null;

            try { awaiter = (IAwaiter)result.Context; }
            catch { Debug.LogWarning($"{this}: instance type is not of awaiter!"); return; }

            if (result.State)
            {
                m_AwaiterIsReady.Add(awaiter);
            }
            else
            {
                if (m_AwaiterIsReady.Contains(awaiter))
                    m_AwaiterIsReady.Remove(awaiter);
            }

            AwaiterLimitUpdate();
        }


        public static AsyncController Get(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
            {
                try
                {
                    var instance = new AsyncController();
                    instance.Init((AsyncControllerConfig)args[config]);
                    return instance;
                }

                catch { Debug.Log("Custom factory not found! The instance will be created by default."); }
            }

            return new AsyncController();
        }

    }

    public interface IAsyncController : IController, IConfigurable, IUpdatable
    {

        event Action<IAsyncInfo> FuncExecuted;

        void Awaite(Func<bool> func);
        void Awaite(Action func);
        void Awaite(float delay);

        void Run();

        IEnumerator ExecuteAsync(IYield func);




    }

    public struct AsyncControllerConfig : IConfig
    {
        public AsyncControllerConfig(GameObject asyncHolder)
        {
            AsyncHolder = asyncHolder;
        }

        public GameObject AsyncHolder { get; private set; }
    }

    public struct FuncAsyncInfo : IAsyncInfo
    {

        public IYield Func { get; private set; }
        public IAwaiter Awaiter { get; set; }



        public FuncAsyncInfo(IYield func, IAwaiter awaiter)
        {
            Func = func;
            Awaiter = awaiter;
        }

        public FuncAsyncInfo(IYield func)
        {
            Func = func;
            Awaiter = null;
        }


    }

    /*
    public struct FuncAsyncInfo : IAsyncInfo
    {
        public IAwaiter Awaiter { get; private set; }
        public Func<Action<bool>, IEnumerator> FuncAsync { get; private set; }

        public FuncAsyncInfo(IAwaiter awaiter, Func<Action<bool>, IEnumerator> func)
        {
            FuncAsync = func;
            Awaiter = awaiter;
        }
    }
    */

    public interface IAsyncInfo
    {
        IAwaiter Awaiter { get; set; }
        IYield Func { get; }

    }


}


/*
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

*/



