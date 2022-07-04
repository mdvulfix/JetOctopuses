using System;

namespace APP
{
    public interface ISession: IConfigurable
    {
        IState StateActive { get; }
    }
}