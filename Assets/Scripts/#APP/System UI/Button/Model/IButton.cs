using System;

namespace APP
{
    public interface IButton : IConfigurable
    {
        event Action<ISignal> ButtonClicked;
    }

}