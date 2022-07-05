using UnityEngine;

namespace APP.Vfx
{

    public class VfxDefault: VfxModel<VfxDefault>, IVfx
    {
        protected override void Init()
        {
            

            var vfxController = new VfxControllerDefault();
            
            
            var config = new VfxConfig(this, vfxController);
            
            base.Configure(config);
            base.Init();
        } 
            
    }



    public class VfxModel<TVfx>: SceneObject, IConfigurable
    {        
        private VfxConfig m_Config;
        private IVfxController m_VfxController;

        public bool IsConfigured {get; private set;}

        public void Configure(IConfig config)
        {
            if(IsConfigured == true)
                return;
            
            
            m_Config = (VfxConfig)config;
            m_VfxController = m_Config.VfxController;

        
            IsConfigured = true;
        }

        protected override void Init() { }
        protected override void Dispose() { }
    }

    public interface IVfx
    {
    }

    public class VfxConfig: Config
    {
        public IVfxController VfxController {get; private set;}

        public VfxConfig(IVfx vfx, IVfxController vfxController): base(vfx)
        {
            VfxController = vfxController;
        }    
    }


}
