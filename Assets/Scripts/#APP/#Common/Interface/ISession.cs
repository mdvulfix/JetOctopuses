using System;

namespace APP
{
    public interface ISession: IConfigurable, IInitializable, ICacheable
    {
        IState StateActive { get; }
    }
}