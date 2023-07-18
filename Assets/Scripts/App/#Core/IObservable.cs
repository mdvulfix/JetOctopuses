namespace Core
{
    public interface IObservable
    {
        void SetObserver(params object[] observers);
        void RemoveObserver(params object[] observers);
    }
}