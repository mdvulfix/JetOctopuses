using System;

namespace APP
{
    public interface IButton : IConfigurable, IInitializable
    {
        event Action<ISignal> ButtonClicked;
    }

}