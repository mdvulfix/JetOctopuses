namespace APP
{
    public interface IScene : IConfigurable, IInitializable, ICacheable
    {
        void Activate<TScreen>() 
            where TScreen: SceneObject, IScreen;

        
    }

}