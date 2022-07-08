using System;

namespace APP
{
    public interface IConfigurable
    {
        event Action Configured;

        bool IsConfigured {get; } 
        
        void Configure(IConfig config = null, params object[] param);

    }

}