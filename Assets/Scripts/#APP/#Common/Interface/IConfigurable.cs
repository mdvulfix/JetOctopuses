namespace APP
{
    public interface IConfigurable
    {
        bool IsConfigured {get; }
        bool IsInitialized {get; }
          
        void Configure(IConfig config);
        void Init();
        void Dispose();
    }

}