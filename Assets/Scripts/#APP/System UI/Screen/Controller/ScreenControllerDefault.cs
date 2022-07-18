using System;
using System.Threading.Tasks;
using SERVICE.Handler;

namespace APP.Screen
{
    public class ScreenControllerDefault : Controller, IScreenController
    {
        private IScreen m_ScreenActive;
        private ScreenControllerConfig m_Config;
       
        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; protected set; }

        public IScreen[] Screens { get; protected set; }
        
        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public ScreenControllerDefault() { }
        public ScreenControllerDefault(IConfig config) => Configure(config);

        public IMessage Configure(IConfig config, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);
            
            if(config != null)
            {
                m_Config = (ScreenControllerConfig) config;
                Screens = m_Config.Screens;

            }          
               
            if(param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            foreach (var screen in Screens)
                if (screen.IsConfigured == false)
                    screen.Configure();
            
            Send("All screens configured!");
            
            IsConfigured = true;
            Configured?.Invoke();
            
            return Send("Configuration completed!");
        }

        public IMessage Init() 
        { 
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);


            foreach (var screen in Screens)
                screen.Init();

            Send("All screens initialized!");
            
            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public IMessage Dispose() 
        { 
            
            foreach (var screen in Screens)
                screen.Dispose();
            
            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
        }

        
        public async Task<ITaskResult> ScreenLoad(IScreen screen)
        {
            if (screen == null)
                return new TaskResult(false, Send($"{screen.GetType().Name} not found!", LogFormat.Worning));

            var result = default(ITaskResult);
            await TaskHandler.Run(() => AwaitScreenLoad(screen, out result), "Waiting for screen loading...");
            return result;
        }
            

        
        public async Task<ITaskResult> ScreenActivate(IScreen screen, bool screenActivate, bool screenAnimate)
        {
            if (screen == null)
                return new TaskResult(false, Send($"{screen.GetType().Name} not found!", LogFormat.Worning));

            
            if (m_ScreenActive != null && m_ScreenActive.GetType() == screen.GetType())
                return new TaskResult(true, Send($"{screen.GetType().Name} is already active."));

            
            var result = default(ITaskResult);
            await TaskHandler.Run(() => AwaitScreenActivation(screen, screenActivate, screenAnimate, out result), "Waiting for screen activation...");
            return result;

        }

        private bool AwaitScreenLoad(IScreen screen, out ITaskResult result)
        {
            result = default(ITaskResult);
            if(screen == null)
                return false;
            
            result = screen.Load().Result;
            return result.Status;
        }

        private bool AwaitScreenActivation(IScreen screen, bool screenActivate, bool screenAnimate, out ITaskResult result)
        {
            result = default(ITaskResult);
            
            if(screen == null)
                return false;
            
            if (m_ScreenActive != null)
            {
                var activeScreenActivate = false;
                var activeScreenAnimate = false;
                m_ScreenActive.Activate(activeScreenActivate, activeScreenAnimate);
            }
                
            m_ScreenActive = screen;
            result = m_ScreenActive.Activate(screenActivate, screenAnimate).Result;
            
            return result.Status;
        }

        // CALLBACK //

    }

    public struct ScreenControllerConfig: IConfig
    {
        public ScreenControllerConfig(IScreen[] screens)
        {
            Screens = screens;
        }

        public IScreen[] Screens {get; private set; }
    }

    public interface IScreenController : IController, IConfigurable, IInitializable
    {
        
        Task<ITaskResult> ScreenLoad(IScreen screen);
        Task<ITaskResult> ScreenActivate(IScreen screen, bool screenActivate, bool screenAnimate);
    }
}