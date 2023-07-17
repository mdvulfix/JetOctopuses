using System;

namespace Core.Cache
{
    public interface ICacheable
    {
        event Action RecordRequired;
        event Action DeleteRequired;




    }
}