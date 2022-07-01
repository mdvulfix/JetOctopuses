namespace APP
{
    public interface IScene : IConfigurable
    {
        void Activate<TScreen>() 
            where TScreen: UComponent, IScreen;
    }

}