
public interface IConfig
{ 
    SceneController SceneController { get; }
    Audio Audio  { get; }
    Vfx Vfx { get; }
}

public struct Config: IConfig
{
    public SceneController SceneController { get; }
    public Audio Audio  { get; }
    public Vfx Vfx { get; }

    public Config (
        SceneController sceneController = null, 
        Audio audio = null, 
        Vfx vfx = null)
    {
        SceneController = sceneController;
        Audio = audio;
        Vfx = vfx;
    }
}