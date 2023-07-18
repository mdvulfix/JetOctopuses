using System;
using System.Collections;
using UnityEngine;
using Core;
using Core.Factory;

namespace Core.Async
{

    public class AwaiterDefault : AwaiterModel, IAwaiter
    {




        public event Action<IAwaiter> FuncInvoked;
        public event Action<IAwaiter> FuncExecuted;


        public static string PREF_NAME = "AwaiterDefault";

        public AwaiterDefault() { }
        public AwaiterDefault(params object[] args)
            => Init(args);


        public override void Init(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Init(args);
                return;
            }

            // CONFIGURE BY DEFAULT //
            Debug.LogWarning($"{this.GetName()}: {Label} will be initialized by default!");


            var config = new AwaiterConfig(Label, null);
            base.Init(config);

        }


        public override void Run(IYield func)
        {
            FuncInvoked?.Invoke(this);
            StartCoroutine(ExecuteAsync(func, (result) =>
            {
                if (result.State)
                    Resolve();

            }));

        }

        public override void Resolve()
        {
            FuncExecuted?.Invoke(this);

        }






        /*
        public override IEnumerator ExecuteAsync(IEnumerator func, Action<IResult> callback)
        {
            SetState(false);
            FuncInvoked?.Invoke(this);

            if (m_Coroutine != null)
                StopCoroutine(m_Coroutine);

            yield return m_Coroutine = StartCoroutine(func);
            callback?.Invoke(new Result(this, true, $"Async operation done!"));

            SetState(true);
            FuncExecuted?.Invoke(this);

        }
        */


        // FACTORY //
        public static AwaiterDefault Get(IFactory factory, params object[] args)
            => Get<AwaiterDefault>(factory, args);
    }



    public partial class AwaiterFactory : Factory<IAwaiter>, IFactory
    {
        private AwaiterDefault GetAwaiterDefault(params object[] args)
        {
            var prefabPath = $"{AwaiterModel.PREF_FOLDER}/{AwaiterDefault.PREF_NAME}";
            var prefab = Resources.Load<GameObject>(prefabPath);

            var obj = (prefab != null) ?
            GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) :
            new GameObject("Awaiter");

            obj.SetActive(false);

            var instance = obj.AddComponent<AwaiterDefault>();
            obj.name = $"Awaiter";

            if (args.Length > 0)
                try { instance.Init((AwaiterConfig)args[(int)AwaiterModel.Params.Config]); }
                catch { Debug.LogWarning($"{this.GetName()}: config was not found. Configuration failed!"); }

            return instance;
        }
    }


    /*

    
    
    
    public class AwaiterDefault : AwaiterModel, IAwaiter
    {

        private GameObject m_Obj;
        private CancellationToken m_Token;



        public static string PREFAB_NAME;


        public AwaiterDefault() { }
        public AwaiterDefault(params object[] args)
           => Init(args);




        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
            {
                try { m_Config = (AwaiterConfig)args[config]; }
                catch { OnInitChanged(new Result(this, false, $"{m_Label} config was not found. Initialization failed!")); return; }
            }

            m_Obj = ObjSelf;

            m_Token = new CancellationToken();
            OnInitChanged(new Result(this, true, $"{m_Label} initialized!"));


        }

        public override void Dispose()
        {

            m_Token.Cancel();
            OnInitChanged(new Result(this, false, $"{m_Label} disposed!"));
        }




        // ACTIVATE //
        public override void Activate()
        {
            m_Obj.SetActive(true);
            OnActivateChanged(new Result(this, true, $"{m_Label} activated!"));
            OnReadyChanged(new Result(this, false, $"{m_Label} is ready!"));

        }

        public override void Deactivate()
        {
            m_Obj.SetActive(false);
            OnReadyChanged(new Result(this, false, $"{m_Label} is not ready!"));
            OnActivateChanged(new Result(this, true, $"{m_Label} deactivated!"));

        }


        public override IResult Run(object context, Func<bool> action)
        {
            if (!m_isReady)
                return new Result(this, false, $"Awaiter {m_Name} is busy...", m_isDebug, LogFormat.Warning);

            var token = new CancellationToken();
            var delay = 5f;

            try { StartCoroutine(ExecuteAsync(action, delay, token)); }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }

            return new Result(context, true);
        }

        private IEnumerator ExecuteAsync(Func<bool> action, float delay, CancellationToken token)
        {
            var result = default(IResult);
            var log = "...";

            m_isReady = false;

            log = $"{this.GetName()}: Async operation started...";
            result = new Result(this, m_isReady, log, m_isDebug);
            Ready?.Invoke(result);


            while (!token.isCancelled)
            {
                if (action.Invoke())
                {
                    token.Cancel();

                    OnReadyCallback(new Result(this, true, $"{m_Label} deactivated!"));



                    m_isReady = true;
                    log = $"{this.GetName()}: Async operation successfully finished.";
                    result = new Result(this, m_isReady, log, m_isDebug);
                    Ready?.Invoke(result);
                    yield return null;

                }

                if (delay <= 0)
                {
                    token.Cancel();

                    m_isReady = true;
                    log = $"{this.GetName()}: Async operation cancelled by time delay.";
                    result = new Result(this, m_isReady, log, m_isDebug);
                    Ready?.Invoke(result);
                    yield return null;

                }

                yield return new WaitForSeconds(0.5f);
                delay -= Time.deltaTime;
            }



            m_isReady = true;
            log = $"{this.GetName()}: Async operation cancelled.";
            result = new Result(this, m_isReady, log, m_isDebug);
            Ready?.Invoke(result);

        }



        // FACTORY //
        public static AwaiterDefault Get(params object[] args)
            => Get<AwaiterDefault>(args);


    }


    public class CancellationToken : IToken
    {

        public bool isCancelled { get; private set; }

        public void Cancel()
        {
            isCancelled = true;
        }

        public void Reset()
        {
            isCancelled = false;
        }

    }

    public interface IToken
    {

    }


    public partial class AwaiterFactory : Factory<IAwaiter>
    {

        private AwaiterDefault GetDefault(params object[] args)
        {

            var prefabPath = $"{AwaiterModel.PREFAB_Folder}/{AwaiterDefault.PREFAB_Label}";
            var prefab = Resources.Load<GameObject>(prefabPath);

            var obj = (prefab != null) ?
            GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) :
            new GameObject("Awaiter");

            obj.SetActive(false);

            var instance = obj.AddComponent<AwaiterDefault>();
            obj.name = $"Awaiter {instance.GetHashCode()} ";

            if (args.Length > 0)
            {
                try { instance.Init((AwaiterConfig)args[(int)AwaiterModel.Params.Config]); }
                catch { Debug.Log("Custom factory not found! The instance will be created by default."); }

            }

            return instance;
        }

    }

    */
}

