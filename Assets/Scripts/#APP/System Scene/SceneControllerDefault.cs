using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;
using SERVICE.Handler;
using APP.Screen;

namespace APP.Scene
{
    public class SceneControllerDefault : Controller, ISceneController
    {
        private SceneControllerConfig m_Config;

        private IScene m_SceneActive;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized {get; protected set; }



        public Action<IScene> SceneActivated;

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        
        public SceneControllerDefault() => Configure();
        public SceneControllerDefault(IConfig config) => Configure(config);

        // CONFIGURE //
        public virtual void Configure() =>
            Configure(config: null);

        public virtual void Configure(IConfig config) =>
            Configure(config: config, param: null);

        public virtual void Configure (IConfig config, params object[] param)
        {
            if(config != null)
            {
                m_Config = (SceneControllerConfig)config;
            }          
               
            if(param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            OnConfigured();
        }
        
        
        
        public void Init()
        {
            if(IsConfigured == false)
            {
                Send("Instance was not configured. Initialization was failed!", LogFormat.Worning);
                return;
            }
            
            
            OnInitialized();
        }

        public void Dispose()
        {


            OnDisposed();
        }





        public async Task Activate<TScene>() 
        where TScene : SceneObject, IScene
        {
            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
            {
                Send($"{typeof(TScene).Name} is already active!");
                return;
            }
            
            var sceneIndex = SceneIndex<TScene>.Index;
            await SceneHandler.Activate(sceneIndex);

            TScene scene = null;
            await TaskHandler.Run(() => AwaitSceneActivation<TScene>(sceneIndex, out scene), "Waiting for scene activation...");

            if (scene == null)
            {
                Send($"{scene.GetType().Name} not found!", LogFormat.Worning);
                return;
            }

            m_SceneActive = scene;
            m_SceneActive.Activate<ScreenLoading>();

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

    public struct SceneControllerConfig : IConfig
    {
        
    }

    public interface ISceneController : IController, IConfigurable, IInitializable
    {
        Task Activate<TScene>()
            where TScene : SceneObject, IScene;

    }
}