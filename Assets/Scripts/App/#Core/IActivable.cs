using System;

namespace Core
{
    public interface IActivable
    {
        bool isActivated { get; }

        event Action<IResult> Activated;
        event Action<IResult> Deactivated;

        void Activate();
        void Deactivate();

    }

}
