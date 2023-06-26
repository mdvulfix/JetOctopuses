using System;
using UnityEngine;

using Core;
using Core.Factory;

namespace Core.Spawn
{
    [Serializable]
    public abstract class SpawnerModel : ModelCommon
    {
        [SerializeField] bool m_Debug = true;

        [Header("Stats")]
        [SerializeField] private bool m_Configured;
        [SerializeField] private bool m_Initialized;

        private SpawnerConfig m_Config;


        public enum Params
        {
            Config,
            Factory
        }

        // CONFIGURE //
        public override void Configure(params object[] args)
        {
            var configIndex = (int)Params.Config;

            if (args.Length > 0)
            {
                try { m_Config = (SpawnerConfig)args[(int)Params.Config]; }
                catch
                {
                    Debug.LogWarning($"{this.GetName()} config was not found. Configuration failed!");
                    return;
                }
            }


            m_Config = (SpawnerConfig)args[configIndex];

            Configured += OnConfigured;
            base.Configure();
        }

        public override void Init()
        {
            //ScenesRegistered.Add(m_Scene);

            Initialized += OnInitialized;
            base.Init();

        }

        public override void Dispose()
        {

            base.Dispose();
            Initialized -= OnInitialized;
        }

        public virtual void Spawn() { }
        public virtual void Spawn<T>() { }

        public virtual void Spawn(GameObject obj, Vector3 pos = default(Vector3))
            => GameObject.Instantiate(obj, pos, Quaternion.identity);

        private void OnConfigured(bool result)
        {
            m_Configured = result;
            if (m_Debug && result) Debug.Log($"{this.GetName()} configured successfully.");

        }

        private void OnInitialized(bool result)
        {
            m_Initialized = result;
            if (m_Debug && result) Debug.Log($"{this.GetName()} initialized successfully.");

        }

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