using UnityEngine;

namespace APP.Audio
{
    public class AudioDefault : AudioModel<AudioDefault>, IAudio
    {
        protected override void Init()
        {
            var audioController = new AudioControllerDefault();

            var config = new AudioConfig(this, audioController);
            base.Configure(config);
            base.Init();
        }

    }


    ////////////////////////////////////////////////////////////
    public class AudioModel<TAudio> : SceneObject, IConfigurable
    {

        private AudioConfig m_Config;
        private IAudioController m_AudioController;

        public bool IsConfigured {get; private set;}

        public virtual void Configure(IConfig config)
        {
            if (IsConfigured == true)
                return;


            m_Config = (AudioConfig)config;
            m_AudioController = m_Config.AudioController;


            IsConfigured = true;
        }

        protected override void Init() { }
        protected override void Dispose() { }

    }

    public interface IAudio
    {

    }

    public class AudioConfig : Config
    {
        public AudioConfig(IAudio audio, IAudioController audioController): base(audio)
        {
            AudioController = audioController;
        }

        public IAudioController AudioController { get; private set; }

    }


}

