namespace SERVICE.Factory
{
    public interface IFactory
    {
        T Get<T>(params object[] p);
    }

}