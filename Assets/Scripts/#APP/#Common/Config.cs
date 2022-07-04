namespace APP
{
    public class Config: IConfig
    {
        public Instance Instance{get; private set;}

        public Config(Instance info) =>
            Instance = info;

    }

}