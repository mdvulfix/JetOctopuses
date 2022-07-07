using System;

namespace APP
{
    public interface IConfigurable
    {
        event Action Configured;

        bool IsConfigured {get; } 
        
        void Configure();
        void Configure(IConfig config);
        void Configure(IConfig config, params object[] param);

        bool CheckConfigure();

    }

}