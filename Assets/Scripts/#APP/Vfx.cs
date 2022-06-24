using UnityEngine;

namespace APP.Vfx
{

    public class VfxDefault: VfxModel<VfxDefault>, IVfx
    {
        protected override void Init() => 
            base.Init();
    }



    public class VfxModel<TVfx>: SceneObject, IConfigurable
    {
        
        public bool IsConfigured {get; private set;}
        
        private VfxConfig m_Config;
        private IVfxController m_VfxController;
        
        public void Configure(IConfig config)
        {
            m_Config = (VfxConfig)config;
            m_VfxController = m_Config.VfxController;

            IsConfigured = true;
        }

        protected override void Init() => 
            base.Init();

        protected override void Dispose() => 
            base.Dispose();
    }

    public interface IVfx
    {
    }

    public struct VfxConfig: IConfig
    {
        public IVfxController VfxController {get; private set;}

        public VfxConfig(IVfxController vfxController)
        {
            VfxController = vfxController;
        }    
    }


}
