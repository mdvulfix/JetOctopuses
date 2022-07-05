namespace APP
{
    
    
    
    
    public struct Config: IConfig
    {
        public object Instance {get; private set;}

        public Config(object instance) =>
            Instance = instance;

    }

}