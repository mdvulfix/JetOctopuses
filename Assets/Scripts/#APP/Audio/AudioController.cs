namespace APP.Audio
{
    public class AudioControllerDefault: Controller, IAudioController, IConfigurable
    {
        private AudioControllerConfig m_Config;
        
        public AudioControllerDefault(IConfig config) =>
            Configure(config);
        
        
        public void Configure(IConfig config)
        {
            m_Config = (AudioControllerConfig)config;
        }
        
        public override void Init() { }
        public override void Dispose() { }


    }

    public interface IAudioController
    {
    }

    public struct AudioControllerConfig: IConfig
    {


    }
}

