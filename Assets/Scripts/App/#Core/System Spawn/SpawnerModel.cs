using System;
using UnityEngine;

using Core;
using Core.Factory;

namespace Core.Spawn
{
    [Serializable]
    public abstract class SpawnerModel : ModelConfigurable
    {

        private bool m_isDebug = true;
        private SpawnerConfig m_Config;

        public string Label => "Awaiter";

        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (SpawnerConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {Label} config was not found. Configuration failed!"); return; }

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

    public struct SpawnerConfig : IConfig
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