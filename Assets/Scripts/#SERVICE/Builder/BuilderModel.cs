using UnityEngine;
using SERVICE.Handler;
using APP;
using APP.Audio;
using APP.Vfx;
using APP.Scene;

namespace SERVICE.Builder
{
    public abstract class BuilderModel<TBuilder> : UComponent
    {
        private BuilderConfig m_Config;
        
        public override void Configure(IConfig config)
        {
            base.Configure(config);
            m_Config = (BuilderConfig)config;
            
        }
        
        
        protected override void Init()
        {
            base.Init();
        }

        protected override void Dispose()
        {
            base.Dispose();
        }


        // BUILD
        public abstract void Build(params IConfig[] param);


    }

    public class BuilderConfig : Config
    {
        public BuilderConfig(InstanceInfo info): base(info)
        {
        }
    }

    public interface IBuilder
    {
        void Build(params IConfig[] parametrs);
    }
}