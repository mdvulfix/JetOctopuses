using System;
using UnityEngine;

using Core;
using Core.Factory;

namespace Core.Spawn
{
    [Serializable]
    public abstract class SpawnerModel : ModelBasic, ISpawner
    {
        [Header("Debug")]
        [SerializeField] bool m_Debug = true;

        [Header("Stats")]
        [SerializeField] bool m_Configured;
        [SerializeField] bool m_Initialized;


        private SpawnerConfig m_Config;


        public event Action<bool> Configured;
        public event Action<bool> Initialized;


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
                try { m_Config = (SpawnerConfig)args[config]; }
                catch { Debug.LogWarning("Scene config was not found. Configuration failed!"); }
                return;
            }


            m_Config = (SpawnerConfig)args[config];


            Configured?.Invoke(m_Configured = true);
            if (m_Debug) Debug.Log($"{this.GetName()} configured.");
        }

        public override void Init()
        {
            //ScenesRegistered.Add(m_Scene);


            Initialized?.Invoke(m_Initialized = true);
            if (m_Debug) Debug.Log($"{this.GetName()} initialized.");

        }

        public override void Dispose()
        {

            Initialized?.Invoke(m_Initialized = false);
            if (m_Debug) Debug.Log($"{this.GetName()} disposed.");
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