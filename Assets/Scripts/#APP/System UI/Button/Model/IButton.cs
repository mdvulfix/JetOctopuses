using System;

using APP.Signal;

namespace APP.Button
{
    public interface IButton : IConfigurable
    {
        event Action<ISignal> ButtonClicked;
    }

}