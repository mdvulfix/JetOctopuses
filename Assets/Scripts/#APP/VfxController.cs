using System;

namespace APP.Vfx
{
    public class VfxControllerDefault: Controller, IConfigurable, IVfxController
    {
        private VfxControllerConfig m_Config;

        public VfxControllerDefault(IConfig config) =>
            Configure(config);


        public void Configure(IConfig config)
        {
            m_Config = (VfxControllerConfig)config;
        }
        
        
        public override void Init(){ }
        public override void Dispose(){ }
    }

    public interface IVfxController
    {
    }

    public struct VfxControllerConfig: IConfig
    {


    }
}
