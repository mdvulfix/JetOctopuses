namespace APP.Audio
{
    public class AudioControllerDefault: Controller, IAudioController
    {       
        public AudioControllerDefault() { }
        
        public override void Init() { }
        public override void Dispose() { }


    }

    public interface IAudioController: IController
    {
        
    }

}

