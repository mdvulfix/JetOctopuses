using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public abstract class ModelButton : Button
    {

        // CONFIGURE //
        public abstract void Init(params object[] args);
        public abstract void Dispose();


    }

}

