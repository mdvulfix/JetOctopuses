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
            => Init(args);


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