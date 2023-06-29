using System;

namespace Core
{
   public interface IActivable
   {
      bool isActivated { get; }

      event Action<IResult> Activated;

      void Activate();
      void Deactivate();

   }

}
