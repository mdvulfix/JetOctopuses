using System;
using System.Collections.Generic;
using UnityEngine;

using Core;
using Core.Factory;


namespace Core.Spawn
{
    [Serializable]
    public class SpawnerDefault : SpawnerModel, ISpawner
    {

        public SpawnerDefault() { }
        public SpawnerDefault(params object[] args)
            => Configure(args);


        public override void Configure(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Configure(args);
                return;
            }

            // CONFIGURE BY DEFAULT //

            var config = new SpawnerConfig();
            base.Configure(config);
            Debug.Log($"{this.GetName()} was configured by default!");

        }


        // FACTORY //
        public static SpawnerDefault Get(params object[] args)
            => Get<SpawnerDefault>(args);


        public partial class SpawnerFactoryDefault : Factory<ISpawner>
        {
            private SpawnerDefault GetSpawnerDefault(params object[] args)
            {
                var instance = new SpawnerDefault(args);
                return instance;
            }
        }
    }
}