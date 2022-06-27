using System;
using System.Threading.Tasks;

namespace APP.Screen
{
    public class ScreenControllerDefault : Controller, IScreenController
    {
        private IScreen m_ScreenActive;

        public ScreenControllerDefault() { }

        public override void Init() { }

        public override void Dispose() { }

        public async Task Activate<TScreen>(bool animate)
        where TScreen : IScreen
        {

            if (m_ScreenActive != null && m_ScreenActive.GetType() == typeof(TScreen))
                return;

            /*
            if (m_CachHandler.Get<TScreen>(out var screen))
            {

                screen.gameObject.SetActive(true);

                if (animate)
                    Animate();
            }
            */

            await Task.Delay(1);

        }

        public void Animate(bool animate = true)
        {

        }

    }

    public interface IScreenController: IController
    {
        Task Activate<TScreen>(bool animate)
        where TScreen : IScreen;
    }
}