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





        protected AwaiterConfig m_Config;
        protected string m_Label = "Awaiter";


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

        public abstract IResult RunAsync(object context, Func<bool> action);


        protected virtual IEnumerator Execute(Func<bool> action, float delay, CancellationToken token)
        {
            if (!m_isReady)
                yield return null;

            OnReady(new Result(this, false, "Async operation started..."));

            while (!token.isCancelled)
            {
                if (action.Invoke())
                {
                    token.Cancel();
                    OnReady(new Result(this, true, "Async operation successfully finished!"));
                    yield return null;
                }

                if (delay <= 0)
                {
                    token.Cancel();
                    OnReady(new Result(this, true, "Async operation cancelled by time delay."));
                    yield return null;

                }

                yield return new WaitForSeconds(0.5f);
                delay -= Time.deltaTime;
            }

            OnReady(new Result(this, true, "Async operation cancelled."));

        }






        protected virtual void OnActivate(IResult result, bool debug = false)
        {
            if (debug)
                Debug.Log($"{result.Context}: {result.Log}");


            m_isActivated = result.Status;
            Activated?.Invoke(result);
        }

        protected virtual void OnReady(IResult result, bool debug = false)
        {
            if (debug)
                Debug.Log($"{result.Context}: {result.Log}");


            m_isReady = result.Status;
            Ready?.Invoke(result);
        }







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

        event Action<IResult> Ready;

        IResult RunAsync(object context, Func<bool> action);

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
