namespace APP
{
    public class Config: IConfig
    {
        public InstanceInfo InstanceInfo{get; private set;}

        public Config(InstanceInfo info) =>
            InstanceInfo = info;

    }

}