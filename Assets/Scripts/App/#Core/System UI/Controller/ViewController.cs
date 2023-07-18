using UnityEngine;

namespace Core.UI
{


    public class ViewController : ModelConfigurable, IViewController
    {

        private bool m_isDebug = true;

        private ViewControllerConfig m_Config;

        public string Label => "ViewController";

        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (ViewControllerConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: config was not found. Configuration failed!"); return; }


        }


    }

    public interface IViewController
    {

    }

    public struct ViewControllerConfig : IConfig
    {

    }

}