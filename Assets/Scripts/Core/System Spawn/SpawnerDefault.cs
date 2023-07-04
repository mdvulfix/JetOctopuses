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


        public override void Init(params object[] args)
        {
            if (args.Length > 0)
            {
                base.Init(args);
                return;
            }

            // CONFIGURE BY DEFAULT //
            $"{this.GetName()} will be initialized by default!".Send(this, m_isDebug, LogFormat.Warning);

            var config = new SpawnerConfig();
            base.Init(config);


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