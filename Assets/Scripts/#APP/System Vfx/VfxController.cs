using System;

namespace APP.Vfx
{
    public class VfxControllerDefault: Controller, IVfxController
    {
        public VfxControllerDefault() {}

        public bool IsConfigured => throw new NotImplementedException();

        public bool IsInitialized => throw new NotImplementedException();

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public void Configure(params object[] param)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
    }

    public interface IVfxController: IController, IConfigurable
    {
        
    }

}
