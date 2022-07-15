using System;

namespace APP
{
    public interface ICacheable: IInitializable
    {
        event Action<ICacheable> RecordToCahceRequired;
        event Action<ICacheable> DeleteFromCahceRequired;



        
    }
}