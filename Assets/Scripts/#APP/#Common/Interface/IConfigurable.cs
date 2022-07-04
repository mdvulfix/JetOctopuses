namespace APP
{
    public interface IConfigurable
    {
        bool IsConfigured {get; }
              
        void Configure(IConfig config);

    }

}