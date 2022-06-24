using UnityEngine;

namespace APP.Audio
{
    public class AudioDefault: AudioModel<AudioDefault>, IAudio
    {
        protected override void Init() => 
            base.Init();
    }

    public class AudioModel<TAudio>: SceneObject, IConfigurable
    {
        
        public bool IsConfigured {get; private set;}
        
        private AudioConfig m_Config;
        private IAudioController m_AudioController;
        
        
        public void Configure(IConfig config)
        {
            m_Config = (AudioConfig)config;
            m_AudioController = m_Config.AudioController;

            IsConfigured = true;
        }

        protected override void Init() => 
            base.Init();

        protected override void Dispose() => 
            base.Dispose();
    }

    public interface IAudio
    {
    }

    public struct AudioConfig: IConfig
    {
        public IAudioController AudioController {get; private set;}

        public AudioConfig(IAudioController audioController)
        {
            AudioController = audioController;
        }
    }


}

