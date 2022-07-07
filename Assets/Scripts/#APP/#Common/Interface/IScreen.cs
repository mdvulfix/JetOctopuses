namespace APP
{
    public interface IScreen : IConfigurable, ICacheable
    {
        void Activate(bool active);
    
    }

}