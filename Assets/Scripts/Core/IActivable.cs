using System;

namespace Core
{
    public interface IActivable
    {
        bool isActivated { get; }

        event Action<bool> Activated;

        IResult Activate();
        IResult Deactivate();

    }

}
