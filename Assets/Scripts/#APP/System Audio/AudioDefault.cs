using UnityEngine;

namespace APP.Audio
{
    public class AudioDefault : AudioModel<AudioDefault>, IAudio
    {
        public override void Init()
        {
            var info = new Instance(this);
            var audioController = new AudioControllerDefault();

            var config = new AudioConfig(info, audioController);
            base.Configure(config);
            base.Init();
        }

    }


    ////////////////////////////////////////////////////////////
    public class AudioModel<TAudio> : UComponent, IConfigurable
    {

        private AudioConfig m_Config;
        private IAudioController m_AudioController;


        public override void Configure(IConfig config)
        {
            if (IsConfigured == true)
                return;

            base.Configure(config);

            m_Config = (AudioConfig)config;
            m_AudioController = m_Config.AudioController;

        }

        public override void Init() =>
            base.Init();

        public override void Dispose() =>
            base.Dispose();
    }

    public interface IAudio
    {

    }

    public class AudioConfig : Config
    {
        public AudioConfig(Instance info, IAudioController audioController): base(info)
        {
            AudioController = audioController;
        }

        public IAudioController AudioController { get; private set; }

    }


}

