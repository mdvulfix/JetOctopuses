using System;

namespace APP.Vfx
{
    public class VfxControllerDefault: Controller, IVfxController
    {
        public VfxControllerDefault() {}

        public override void Init(){ }
        public override void Dispose(){ }
    }

    public interface IVfxController: IController
    {
        
    }

}
