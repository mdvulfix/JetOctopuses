using System;

namespace APP
{
    public interface IConfigurable
    {
        event Action Configured;

        bool IsConfigured {get; } 
        
        IMessage Configure(IConfig config = null, params object[] param);

    }

}