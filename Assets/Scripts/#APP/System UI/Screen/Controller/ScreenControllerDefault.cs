using System;
using System.Threading.Tasks;
using SERVICE.Handler;

namespace APP.Screen
{
    public class ScreenControllerDefault : Controller, IScreenController
    {
        private IScreen m_ScreenActive;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; protected set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public ScreenControllerDefault() { }
        public ScreenControllerDefault(IConfig config) =>
            Configure(config);

        public void Configure(IConfig config)
        {
            if (IsConfigured == true)
                return;

            OnConfigured();
        }

        public void Init() 
        { 
            
            
            OnInitialized(); 
        }

        public void Dispose() 
        { 
            
            
            OnDisposed(); 
        }

        public async Task Activate<TScreen>(bool animate)
        where TScreen : SceneObject, IScreen
        {

            if (m_ScreenActive != null && m_ScreenActive.GetType() == typeof(TScreen))
                return;

            TScreen screen = null;
            await TaskHandler.Run(() => AwaitScreenActivation<TScreen>(out screen), "Waiting for screen activation...");

            if (screen == null)
            {
                Send($"{screen.GetType().Name} not found!", LogFormat.Worning);
                return;
            }

            if (m_ScreenActive != null)
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
        where TScreen : SceneObject, IScreen
        {
            screen = default(TScreen);

            if (СacheProvider<TScreen>.Contains())
            {
                screen = СacheProvider<TScreen>.Get();
                return true;
            }

            return false;
        }

        public void Animate(bool animate = true)
        {

        }

        // CALLBACK //
        private void OnConfigured()
        {
            Send($"Configuration successfully completed!");
            IsConfigured = true;
            Configured?.Invoke();
        }

        private void OnInitialized()
        {
            Send($"Initialization successfully completed!");
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDisposed()
        {
            Send($"Dispose process successfully completed!");
            IsInitialized = false;
            Disposed?.Invoke();
        }

    }

    public interface IScreenController : IController, IConfigurable, IInitializable
    {
        Task Activate<TScreen>(bool animate)
        where TScreen : SceneObject, IScreen;
    }
}