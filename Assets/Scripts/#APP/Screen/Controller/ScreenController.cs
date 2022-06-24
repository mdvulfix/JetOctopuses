using System;

namespace APP.Screen
{
    public class ScreenController : Controller, IConfigurable
    {
        private IScreen m_ScreenActive;

        public ScreenController(IConfig config) =>
            Configure(config);

        public void Configure(IConfig config)
        {

        }

        public override void Init() { }

        public override void Dispose() { }

        public bool Activate<TScreen>(bool animate)
        where TScreen : IScreen
        {

            if (m_ScreenActive != null && m_ScreenActive.GetType() == typeof(TScreen))
                return true;

            /*
            if (m_CachHandler.Get<TScreen>(out var screen))
            {

                screen.gameObject.SetActive(true);

                if (animate)
                    Animate();
            }
            */

            return true;
        }

        public void Animate(bool animate = true)
        {

        }

    }

    public class ScreenControllerConfig : IConfig
    {
        public ScreenControllerConfig()
        {

        }
    }

}