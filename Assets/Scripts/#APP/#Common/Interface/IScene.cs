namespace APP
{
    public interface IScene : IConfigurable
    {
        SceneIndex SceneIndex { get; }
        
        void Activate<TScreen>() 
            where TScreen: UComponent, IScreen;
    }

}