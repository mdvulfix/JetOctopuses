using System;
using System.Threading.Tasks;
using UScene = UnityEngine.SceneManagement.Scene;
using SERVICE.Handler;
using APP.Screen;
using APP.Signal;

namespace APP.Scene
{
    public class SceneControllerDefault : Controller, ISceneController
    {

        private SceneControllerConfig m_Config;
        
        private ISignal m_SignalSceneActivate;
        
        
        public bool IsConfigured { get; private set; }
        public bool IsInitialized {get; protected set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<IScene> SceneLoaded;
        public event Action<IScene> SceneActivated;
        
        
        public SceneControllerDefault(params object[] param) => Configure(param);
        public SceneControllerDefault() { }

        // CONFIGURE //
        public virtual void Configure (params object[] param)
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
                        m_Config = (SceneControllerConfig) obj;
                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            

            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }
        
        public virtual void Init()
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
                
            Subscribe();

            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {

            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }

        // SUBSCRIBE //
        public virtual void Subscribe()
        {
            SignalProvider.SignalCalled += OnSignalCalled;
        }

        public virtual void Unsubscribe()
        {
            SignalProvider.SignalCalled -= OnSignalCalled;
        }
    

        public async Task<ITaskResult> SceneLoad(IScene scene)
        {
            if (scene == null)
                return new TaskResult(false, Send($"{scene.GetType().Name} not found!", LogFormat.Worning));

            if (scene.IsLoaded == true)
                return new TaskResult(true, Send($"{scene.GetType().Name} is already loaded!"));

            

            

            
            var sceneLoadingTaskResult = await scene.Load();
            if(sceneLoadingTaskResult.Status == false)
                return new TaskResult(false, sceneLoadingTaskResult.Message);

            
            SceneLoaded?.Invoke(scene);
            return new TaskResult(true, Send($"{scene.GetType().Name} was loaded!"));
        }

        public async Task<ITaskResult> SceneActivate(IScene scene, IScreen screen, bool screenActivate, bool screenAnimate)
        {
            if (scene == null)
                return new TaskResult(false, Send($"{scene.GetType().Name} not found!", LogFormat.Worning));

            if (scene.IsActivated == true)
                return new TaskResult(true, Send($"{scene.GetType().Name} is already activated!"));
            
            var uSceneActivationTaskResult = await SceneHandler.USceneActivate(scene.Index);
            if(uSceneActivationTaskResult.Status == false)
                return new TaskResult(false, uSceneActivationTaskResult.Message);
            
            var sceneActivationTaskResult = await scene.Activate(screen, screenActivate, screenAnimate);
            if(sceneActivationTaskResult.Status == false)
                return new TaskResult(false, sceneActivationTaskResult.Message);

            SceneActivated?.Invoke(scene);
            return new TaskResult(true, Send($"{scene.GetType().Name} was activated!"));
        }


        // CALLBACK //
        private void OnSignalCalled(ISignal signal)
        {
            
            //if(signal is SignalSceneActivate)
                
            
        }


        private void OnSignalCached(ISignal signal)
        {
            
            //if(signal is SignalSceneActivate)
            //    SceneAc
            
        }




    }

    public struct SceneControllerConfig : IConfig
    {

    }


}

namespace APP
{
    public interface ISceneController : IController, IConfigurable, ISubscriber, IMessager
    {
        Task<ITaskResult> SceneLoad(IScene scene);
        Task<ITaskResult> SceneActivate(IScene scene, IScreen screen, bool screenActivate, bool screenAnimate);
    }
}