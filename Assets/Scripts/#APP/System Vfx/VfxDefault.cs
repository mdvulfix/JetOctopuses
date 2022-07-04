using UnityEngine;

namespace APP.Vfx
{

    public class VfxDefault: VfxModel<VfxDefault>, IVfx
    {
        public override void Init()
        {
            
            var info = new Instance(this);
            var vfxController = new VfxControllerDefault();
            
            
            var config = new VfxConfig(info, vfxController);
            
            base.Configure(config);
            base.Init();
        } 
            
    }



    public class VfxModel<TVfx>: UComponent
    {        
        private VfxConfig m_Config;
        private IVfxController m_VfxController;
        
        public override void Configure(IConfig config)
        {
            if(IsConfigured == true)
                return;
            
            base.Configure(config);
            
            m_Config = (VfxConfig)config;
            m_VfxController = m_Config.VfxController;

        }

        public override void Init() => 
            base.Init();

        public override void Dispose() => 
            base.Dispose();
    }

    public interface IVfx
    {
    }

    public class VfxConfig: Config
    {
        public IVfxController VfxController {get; private set;}

        public VfxConfig(Instance info, IVfxController vfxController): base(info)
        {
            VfxController = vfxController;
        }    
    }


}
