using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SERVICE.Handler;

namespace APP.Screen
{
    public class ScreenControllerDefault : Controller, IScreenController
    {
        private ScreenControllerConfig m_Config;
       
        private List<IScreen> m_ScreensLoaded;
        private IScreen m_ScreenLoading;
        private IScreen m_ScreenDefault;
        
        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; protected set; }



        public IScreen ScreenActive { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public ScreenControllerDefault(params object[] param) => Configure(param);
        public ScreenControllerDefault() { }

        
        // CONFIGURE //
        public void Configure(params object[] param)
        {      
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Worning);
                return;
            }
                
            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (ScreenControllerConfig) obj;
                        
                        m_ScreenLoading = m_Config.ScreenLoading;
                        m_ScreenDefault = m_Config.ScreenDefault;

                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }


            m_ScreensLoaded = new List<IScreen>();

            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }

        public void Init() 
        { 
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Worning);
                return;
            }
                
            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Worning);
                return;
            }
            
            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public void Dispose() 
        { 
            
            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }

        
        public async Task<ITaskResult> ScreenLoad(IScreen screen)
        {
            if (screen == null)
                return new TaskResult(false, Send($"{screen.GetType().Name} not found!", LogFormat.Worning));

            if (screen.IsLoaded == true)
                return new TaskResult(true, Send($"{screen.GetName()} is already loaded.", LogFormat.Worning));

            var screenLoadTaskResult = await screen.Load();
            if(screenLoadTaskResult.Status == false)
                return new TaskResult(false, Send(screenLoadTaskResult.Message));
                
            m_ScreensLoaded.Add(screen);
            return new TaskResult(true, Send($"{screen.GetName()} was loaded."));
        }
            
        public async Task<ITaskResult> ScreenActivate(IScreen screen, bool animate)
        {
            if (screen == null)
                return new TaskResult(false, Send($"{screen.GetType().Name} not found!", LogFormat.Worning));

            if (screen.IsActivated == true)
                return new TaskResult(true, Send($"{screen.GetName()} is already activated.", LogFormat.Worning));

            var screenLoadingActivateTaskResult = await m_ScreenLoading.Activate(animate);
            if (screenLoadingActivateTaskResult.Status == false)
                return new TaskResult(false, Send(screenLoadingActivateTaskResult.Message));

            if (ScreenActive != null && ScreenActive != screen)
            {
                var screenDeactivateTaskResult = await ScreenActive.Deactivate();
                if (screenDeactivateTaskResult.Status == false)
                    return new TaskResult(false, Send(screenDeactivateTaskResult.Message));
            }
            
            ScreenActive = m_ScreenLoading;

            var screenLoadTaskResult = await screen.Activate(animate);
            if (screenLoadTaskResult.Status == false)
                return new TaskResult(false, Send(screenLoadTaskResult.Message));

            var screenLoadingDeactivateTaskResult = await m_ScreenLoading.Deactivate();
            if (screenLoadingDeactivateTaskResult.Status == false)
                return new TaskResult(false, Send(screenLoadingDeactivateTaskResult.Message));
    
            ScreenActive = screen;


            return new TaskResult(true, Send($"{screen.GetName()} was activated."));
        }

        public async Task<ITaskResult> ScreenDeactivate(IScreen screen)
        {
            if (screen == null)
                return new TaskResult(false, Send($"{screen.GetType().Name} not found!", LogFormat.Worning));

            var screenLoadingActivateTaskResult = await m_ScreenLoading.Activate(true);
            if (screenLoadingActivateTaskResult.Status == false)
                return new TaskResult(false, Send(screenLoadingActivateTaskResult.Message));


            var screenDeactivateTaskResult = await screen.Deactivate();
            if (screenDeactivateTaskResult.Status == false)
                return new TaskResult(false, Send(screenDeactivateTaskResult.Message));

            ScreenActive = m_ScreenLoading;
            
            return new TaskResult(true, Send($"{screen.GetName()} was deactivated."));
        }

        public async Task<ITaskResult> ScreenUnload(IScreen screen)
        {
            if (screen == null)
                return new TaskResult(false, Send($"{screen.GetType().Name} not found!", LogFormat.Worning));

            if(screen.IsLoaded != true)
                return new TaskResult(false, Send($"{screen.GetName()} is already unloaded.", LogFormat.Worning));

            var screenLoadingActivateTaskResult = await m_ScreenLoading.Activate(true);
            if (screenLoadingActivateTaskResult.Status == false)
                return new TaskResult(false, Send(screenLoadingActivateTaskResult.Message));

            var screenDeactivateTaskResult = await screen.Deactivate();
            if (screenDeactivateTaskResult.Status == false)
                return new TaskResult(false, Send(screenDeactivateTaskResult.Message));
 
            ScreenActive = m_ScreenLoading;

            var screenUnloadTaskResult = await screen.Unload();
            if(screenUnloadTaskResult.Status == false)
                return new TaskResult(false, Send(screenUnloadTaskResult.Message));
 
            m_ScreensLoaded.Remove(screen);
            return new TaskResult(true, Send($"{screen.GetName()} was unloaded."));
        }
    }

    public struct ScreenControllerConfig: IConfig
    {
        public ScreenControllerConfig(
            IScreen[] screens,
            IScreen screenLoading,
            IScreen screenDefault)
        {
            Screens = screens;
            ScreenLoading = screenLoading;
            ScreenDefault = screenDefault;
        }

        public IScreen[] Screens {get; private set; }
        public IScreen ScreenLoading { get; private set; }
        public IScreen ScreenDefault { get; private set; }

    }

    public interface IScreenController : IController, IConfigurable, IMessager
    {
        IScreen ScreenActive { get; }

        Task<ITaskResult> ScreenLoad(IScreen screen);
        Task<ITaskResult> ScreenActivate(IScreen screen, bool animate);
        Task<ITaskResult> ScreenDeactivate(IScreen screen);
        Task<ITaskResult> ScreenUnload(IScreen screen);
    }
}