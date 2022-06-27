using System;

namespace APP.Screen
{
    public class ScreenControllerDefault : Controller, IScreenController
    {
        private IScreen m_ScreenActive;

        public ScreenControllerDefault() { }

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

    public interface IScreenController: IController
    {
        bool Activate<TScreen>(bool animate)
        where TScreen : IScreen;
    }
}