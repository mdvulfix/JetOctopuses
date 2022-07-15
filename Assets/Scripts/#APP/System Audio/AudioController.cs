using System;

namespace APP.Audio
{
    public class AudioControllerDefault: Controller, IAudioController
    {       
        public AudioControllerDefault() { }

        public bool IsConfigured => throw new NotImplementedException();

        public bool IsInitialized => throw new NotImplementedException();

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;


        public IMessage Configure(IConfig config, params object[] param)
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

    public interface IAudioController: IController, IConfigurable
    {
        
    }

}

