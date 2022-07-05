namespace APP
{
    public interface IScene : IConfigurable, ICacheable
    {
        void Activate<TScreen>() 
            where TScreen: SceneObject, IScreen;

        
    }

}