using UnityEngine;
using Core;
using Core.Factory;
using System.Collections;
using System;

namespace Core.Async
{
    public class AwaiterDefault : AwaiterModel, IAwaiter
    {

        private Transform m_Transform;
        private string m_Name;


        private TokenCancel m_Token;



        public static string PREFAB_Label;


        [SerializeField] private bool m_isReady;


        public bool isReady => m_isReady;

        public event Action<IAwaiter> FuncStarted;
        public event Action<IAwaiter> FuncCompleted;

        public AwaiterDefault() { }
        public AwaiterDefault(params object[] args)
           => Configure(args);


        public override void Configure(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Configure(args);
                return;
            }


            // DEFAULT CONFIG //
            Debug.LogWarning($"{this.GetName()} will be configured by default!");

            var config = new AwaiterConfig();
            base.Configure(config);
        }

        public override void Init()
        {
            m_Transform = transform;
            m_Name = m_Transform.name;

            m_Token = new TokenCancel();
            m_isReady = true;

            base.Init();
        }

        public override void Dispose()
        {
            m_isReady = false;
            m_Token.Cancel();

            base.Dispose();
        }


        public override void Activate()
        {
            var activate = true;
            m_Transform.gameObject.SetActive(activate);

            base.Activate();
        }

        public override void Deactivate()
        {
            if (!isActivated)
                return;


            var activate = false;
            m_Transform.gameObject.SetActive(activate);

            base.Activate();
        }



        public override IResult Run(object context, Func<bool> action)
        {
            if (!m_isReady) return new Result(this, false, $"Awaiter {m_Name} is busy...", m_isDebug, LogFormat.Warning);

            var token = new TokenCancel();
            var delay = 5f;

            try { StartCoroutine(ExecuteAsync(action, delay, token)); }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }

            return new Result(context, true);
        }

        private IEnumerator ExecuteAsync(Func<bool> action, float delay, TokenCancel token)
        {
            m_isReady = false;
            FuncStarted?.Invoke(this as IAwaiter);
            Debug.Log("Async operation started...");

            while (!token.isCancelled)
            {
                if (action.Invoke())
                {
                    Debug.Log("Async operation successfully finished.");
                    m_isReady = true;
                    FuncCompleted?.Invoke(this as IAwaiter);
                    token.Cancel();
                }

                if (delay <= 0)
                    throw new Exception("Async operation cancelled by time delay.");



                yield return new WaitForSeconds(0.5f);
                delay -= Time.deltaTime;
            }

            Debug.Log("Async operation cancelled.");
            m_isReady = true;
            FuncCompleted?.Invoke(this as IAwaiter);
        }



        // FACTORY //
        public static AwaiterDefault Get(params object[] args)
            => Get<AwaiterDefault>(args);


    }


    public class TokenCancel : IToken
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
                var config = (AwaiterConfig)args[(int)AwaiterModel.Params.Config];
                instance.Configure(config);
                instance.Init();
            }

            return instance;
        }

    }

}
