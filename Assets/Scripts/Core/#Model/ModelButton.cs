using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
   public abstract class ModelButton : Button
   {

      // CONFIGURE //
      public abstract void Configure(params object[] args);
      public abstract void Init();
      public abstract void Dispose();


   }

}

