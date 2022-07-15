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
        
        private IScene m_SceneActive;

        private ISignal m_SignalSceneActivate;
        
        
        public bool IsConfigured { get; private set; }
        public bool IsInitialized {get; protected set; }

        public IScene[] Scenes {get; protected set; }
        
        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<IScene> SceneActivated;
        
        public SceneControllerDefault() { }
        public SceneControllerDefault(IConfig config) => Configure(config);

        // CONFIGURE //
        public virtual IMessage Configure (IConfig config = null, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);
            
            
            if(config != null)
            {
                m_Config = (SceneControllerConfig)config;

                Scenes = m_Config.Scenes;
            }          
               
            if(param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            foreach (var scene in Scenes)
                if(scene.IsConfigured == false)
                    scene.Configure();
                    
            Send("All scenes configured!");


            
            
            IsConfigured = true;
            Configured?.Invoke();
            
            return Send("Configuration completed!");
        }
        
        // INIT //
        public virtual IMessage Init()
        {
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);
            

            foreach (var scene in Scenes)
                scene.Init();

            Send("All scenes initialized!");

            
            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {
            foreach (var scene in Scenes)
                scene.Dispose();

            Send("All scenes disposed!");

            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
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
    
        public async Task SceneActivate(IScene scene, IScreen screen, bool screenActivate, bool screenAnimate)
        {
            if (scene == null)
            {
                Send($"{scene.GetType().Name} not found!", LogFormat.Worning);
                return;
            }
            
            if (m_SceneActive != null && m_SceneActive.GetType() == scene.GetType())
            {
                Send($"{scene.GetType().Name} is already active!");
                return;
            }
            
            await SceneHandler.USceneActivate(scene.Index);
            
            m_SceneActive = scene;
            await m_SceneActive.Activate(screen, screenActivate, screenAnimate);

            //TScene scene = null;
            //await TaskHandler.Run(() => AwaitSceneActivation<TScene>(sceneIndex, out scene), "Waiting for scene activation...");



            
            //m_SceneActive.Activate<ScreenLoading>();

        }



        private bool AwaitSceneActivation<TScene>(SceneIndex? index, out TScene scene) where TScene : SceneObject, IScene
        {
            scene = default(TScene);
            if (СacheProvider<TScene>.Contains())
            {
                scene = СacheProvider<TScene>.Get();
                return true;
            }

            return false;
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
        public IScene[] Scenes {get; private set; }

        public SceneControllerConfig(IScene[] scenes)
        {
            Scenes = scenes;
        }
    }


}

namespace APP
{
    public interface ISceneController : IController, IConfigurable, IInitializable, ISubscriber, IMessager
    {
        Task SceneActivate(IScene scene, IScreen screen, bool screenActivate, bool screenAnimate);
    }
}