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

        public event Action<bool> Configured;
        public event Action<bool> Initialized;
        public event Action<bool> Activated;


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

            if (args.Length > 0)
            {
                try { m_Config = (AwaiterConfig)args[config]; }
                catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug); return; }
            }


            m_Config = (AwaiterConfig)args[config];


            m_isConfigured = true;
            Configured?.Invoke(m_isConfigured);
            $"{this.GetName()} configured.".Send(this, m_isDebug);
        }

        public override void Init()
        {

            m_isInitialized = true;
            Initialized?.Invoke(m_isInitialized);
            $"{this.GetName()} initialized.".Send(this, m_isDebug);

        }

        public override void Dispose()
        {

            m_isInitialized = false;
            Initialized?.Invoke(m_isInitialized);
            $"{this.GetName()} disposed.".Send(this, m_isDebug);
        }


        // ACTIVATE //
        public virtual void Activate()
        {

            m_isActivated = true;
            Activated?.Invoke(m_isActivated);
            $"{this.GetName()} activated.".Send(this, m_isDebug);
        }

        public virtual void Deactivate()
        {





            m_isActivated = false;
            Activated?.Invoke(m_isActivated);
            $"{this.GetName()} deactivated.".Send(this, m_isDebug);
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

        event Action<IAwaiter> FuncStarted;
        event Action<IAwaiter> FuncCompleted;

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
