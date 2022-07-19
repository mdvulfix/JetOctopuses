using System;

namespace APP
{
    public interface ICacheable
    {
        event Action RecordRequired;
        event Action DeleteRequired;



        
    }
}