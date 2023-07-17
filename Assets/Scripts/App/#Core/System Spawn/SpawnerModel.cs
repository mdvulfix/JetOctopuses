using System;
using UnityEngine;

using Core;
using Core.Factory;

namespace Core.Spawn
{
    [Serializable]
    public abstract class SpawnerModel : ModelBasic
    {
        [Header("Stats")]
        [SerializeField] private bool m_isInitialized;


        [Header("Debug")]
        [SerializeField] protected bool m_isDebug = true;

        [Header("Config")]
        [SerializeField] protected SpawnerConfig m_Config;


        public event Action<IResult> Initd;
        public event Action<IResult> Initialized;


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
                try { m_Config = (SpawnerConfig)args[config]; }
                catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
            }




            m_isInitialized = true;
            log = $"{this.GetName()} initialized.";
            result = new Result(this, m_isInitialized, log, m_isDebug);
            Initialized?.Invoke(result);

        }

        public override void Dispose()
        {
            var result = default(IResult);
            var log = "...";



            m_isInitialized = false;
            log = $"{this.GetName()} disposed.";
            result = new Result(this, m_isInitialized, log, m_isDebug);
            Initialized?.Invoke(result);

        }



        public virtual void Spawn() { }
        public virtual void Spawn<T>() { }

        public virtual void Spawn(GameObject obj, Vector3 pos = default(Vector3))
            => GameObject.Instantiate(obj, pos, Quaternion.identity);


        // FACTORY //
        public static T Get<T>(params object[] args)
        where T : ISpawner
        {
            IFactory factoryCustom = null;

            if (args.Length > 0)
            {
                try { factoryCustom = (IFactory)args[(int)Params.Factory]; }
                catch { Debug.Log("Custom factory not found! The instance will be created by default."); }
            }

            var factory = (factoryCustom != null) ? factoryCustom : new FactoryDefault();
            var instance = factory.Get<T>(args);

            return instance;
        }



    }

    public class SpawnerConfig : AConfig, IConfig
    {

    }
}


namespace Core
{
    public interface ISpawner : IConfigurable
    {
        void Spawn();
        void Spawn(GameObject obj, Vector3 pos);
        void Spawn<T>();

    }

}