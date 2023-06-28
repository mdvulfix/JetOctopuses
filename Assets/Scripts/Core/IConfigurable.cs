using System;


namespace Core
{
   public interface IConfigurable : IDisposable
   {

      event Action<IEvent> Configured;
      event Action<IEvent> Initialized;

      void Configure(params object[] args);
      void Init();
   }
}

