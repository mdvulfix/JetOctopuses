using System;
using System.Threading.Tasks;
using SERVICE.Handler;

namespace APP.Screen
{
    public class ScreenControllerDefault : Controller, IScreenController
    {
        private IScreen m_ScreenActive;

        public ScreenControllerDefault() { }

        public override void Init() { }

        public override void Dispose() { }

        public async Task Activate<TScreen>(bool animate)
        where TScreen: UComponent, IScreen
        {

            if (m_ScreenActive != null && m_ScreenActive.GetType() == typeof(TScreen))
                return;

            TScreen screen = null;
            await TaskHandler.Run(() => AwaitScreenActivation<TScreen>(out screen), "Waiting for screen activation...");
            
            if(screen == null)
            {
                Send($"{screen.GetType().Name} not found!", true);
                return;
            }
                
            if(m_ScreenActive != null)
                m_ScreenActive.Activate(false);
            
            screen.Activate(true);
            m_ScreenActive = screen;
           

            //Send($"{sceneIndex} activated...");
            
            
            
            /*
            if (m_CachHandler.Get<TScreen>(out var screen))
            {

                screen.gameObject.SetActive(true);

                if (animate)
                    Animate();
            }
            */

            //await Task.Delay(1);

        }

        private bool AwaitScreenActivation<TScreen>(out TScreen screen)
        where TScreen: UComponent, IScreen
        {
            screen = null;
            
            if (RegisterHandler.Contains<TScreen>())
            {
                screen = RegisterHandler.Get<TScreen>();
                return true;
            }
                
            return false;
        }

        public void Animate(bool animate = true)
        {

        }

    }

    public interface IScreenController: IController
    {
        Task Activate<TScreen>(bool animate)
        where TScreen: UComponent, IScreen;
    }
}