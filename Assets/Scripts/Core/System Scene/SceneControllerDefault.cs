using System;
using System.Threading.Tasks;
using UScene = UnityEngine.SceneManagement.Scene;
using Core;
using Core.Signal;


namespace App.Scene
{

    /*
    public class SceneControllerDefault : AController//, ISceneController
    {
        
        private SceneControllerConfig m_Config;

        private IScene m_SceneCore;

        private ISignal m_SignalSceneActivate;


        public IScene SceneActive { get; private set; }

        public event Action<IScene> SceneLoaded;
        public event Action<IScene> SceneActivated;

        public SceneControllerDefault() { }
        public SceneControllerDefault(params object[] param) => Configure(param);

        // CONFIGURE //
        public override void Configure(params object[] param)
        {
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Warning);
                return;
            }

            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (SceneControllerConfig)obj;

                        m_SceneCore = m_Config.SceneCore;

                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Warning);
            }


            //IsConfigured = true;
            //Configured?.Invoke();

            Send("Configuration completed!");
        }

        public override void Init()
        {
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Warning);
                return;
            }

            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Warning);
                return;
            }

            Subscribe();

            //IsInitialized = true;
            //Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public override void Dispose()
        {

            Unsubscribe();

            //IsInitialized = false;
            //Disposed?.Invoke();
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
                return new TaskResult(false, Send($"{scene.GetName()} not found!", LogFormat.Warning));

            if (scene.IsLoaded == true)
                return new TaskResult(true, Send($"{scene.GetName()} is already loaded!"));


            var sceneTargetLoadTaskResult = await scene.Load();
            if (sceneTargetLoadTaskResult.Status == false)
                return new TaskResult(false, sceneTargetLoadTaskResult.Message);


            SceneLoaded?.Invoke(scene);
            return new TaskResult(true, Send($"{scene.GetName()} was loaded!"));
        }

        public async Task<ITaskResult> SceneActivate(IScene scene, bool animate)
        {
            if (scene == null)
                return new TaskResult(false, Send($"{scene.GetName()} not found!", LogFormat.Warning));

            if (scene.IsLoaded == false)
                return new TaskResult(false, Send($"{scene.GetName()} is not loaded.", LogFormat.Warning));

            if (scene.IsActivated == true)
                return new TaskResult(true, Send($"{scene.GetName()} is already activated.", LogFormat.Warning));

            if (SceneActive != null && SceneActive != scene)
            {
                var sceneActiveDeactivateTaskResult = await SceneActive.Deactivate();
                if (sceneActiveDeactivateTaskResult.Status == false)
                    return new TaskResult(false, Send(sceneActiveDeactivateTaskResult.Message));
            }


            if (SceneActive == scene)
                return new TaskResult(true, Send($"{scene.GetName()} was activated."));

            var sceneTargetActivateTaskResult = await scene.Activate(animate);
            if (sceneTargetActivateTaskResult.Status == false)
                return new TaskResult(false, Send(sceneTargetActivateTaskResult.Message));

            var sceneCoreDeactivateTaskResult = await m_SceneCore.Deactivate();
            if (sceneCoreDeactivateTaskResult.Status == false)
                return new TaskResult(false, Send(sceneCoreDeactivateTaskResult.Message));

            SceneActive = scene;

            return new TaskResult(true, Send($"{scene.GetName()} was activated."));
        
        
        
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
        public IScene SceneCore { get; internal set; }
    }

    */
}

namespace App
{
    public interface ISceneController : IController, IConfigurable, ISubscriber, IMessager
    {
        Task<ITaskResult> SceneLoad(IScene scene);
        Task<ITaskResult> SceneActivate(IScene scene, bool animate);
    }
}