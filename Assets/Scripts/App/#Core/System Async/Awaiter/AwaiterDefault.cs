using UnityEngine;
using Core;
using Core.Factory;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Core.Async
{
    public class AwaiterDefault : AwaiterModel, IAwaiter
    {

        [Header("Debug")]
        [SerializeField] private bool m_isDebug = true;

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
                catch { Debug.Log($"{this}: {m_Label} config was not found. Initialization failed!"); return; }

            }

            m_Obj = ObjSelf;

            m_Token = new CancellationToken();
            OnInitialize(new Result(this, true, $"{m_Label} initialized!"), m_isDebug);


        }

        public override void Dispose()
        {

            m_Token.Cancel();
            OnInitialize(new Result(this, false, $"{m_Label} disposed!"), m_isDebug);
        }




        // ACTIVATE //
        public override void Activate()
        {
            m_Obj.SetActive(true);
            OnActivate(new Result(this, true, $"{m_Label} activated!"), m_isDebug);
            OnReady(new Result(this, false, $"{m_Label} is ready!"), m_isDebug);

        }

        public override void Deactivate()
        {
            m_Obj.SetActive(false);
            OnReady(new Result(this, false, $"{m_Label} is not ready!"), m_isDebug);
            OnActivate(new Result(this, true, $"{m_Label} deactivated!"), m_isDebug);

        }


        public override IResult RunAsync(object context, Func<bool> action)
        {
            var token = new CancellationToken();
            var delay = 5f;

            try { StartCoroutine(Execute(action, delay, token)); }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }

            return new Result(context, true);
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

            var prefabPath = $"{AwaiterModel.PREFAB_FOLDER}/{AwaiterDefault.PREFAB_NAME}";
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

}
