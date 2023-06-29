using System;


namespace Core
{
   public interface IConfigurable : IDisposable
   {

      event Action<IResult> Configured;
      event Action<IResult> Initialized;

      void Configure(params object[] args);
      void Init();
   }
}

