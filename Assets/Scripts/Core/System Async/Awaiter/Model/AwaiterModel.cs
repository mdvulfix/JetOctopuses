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
        [SerializeField] private bool m_isInitialized;
        [SerializeField] private bool m_isActivated;
        [SerializeField] private bool m_isReady;


        [Header("Debug")]
        [SerializeField] private bool m_isDebug = true;


        protected AwaiterConfig m_Config;
        protected string m_Label = "Awaiter";


        public bool isActivated => m_isActivated;

        public event Action<IResult> Initialized;
        public event Action<IResult> Activated;
        public event Action<IResult> Ready;


        public static string PREFAB_FOLDER;

        public enum Params
        {
            Config,
            Factory
        }





        public abstract void Activate();
        public abstract void Deactivate();




        protected virtual void OnInitChanged(IResult result)
        {
            if (m_isDebug) Debug.Log($"{result.Context}: {result.Log}");

            m_isInitialized = result.Status;
            Initialized?.Invoke(result);

        }

        protected virtual void OnActivateChanged(IResult result)
        {
            if (m_isDebug) Debug.Log($"{result.Context}: {result.Log}");


            m_isActivated = result.Status;
            Activated?.Invoke(result);
        }

        protected virtual void OnReadyChanged(IResult result)
        {
            if (m_isDebug) Debug.Log($"{result.Context}: {result.Log}");


            m_isReady = result.Status;
            Ready?.Invoke(result);
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

    public interface IAwaiter : IComponent, IConfigurable, IActivable, IPoolable
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
