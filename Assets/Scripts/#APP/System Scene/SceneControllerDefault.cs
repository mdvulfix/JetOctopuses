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



        public Action<IScene> SceneActivated;

        public SceneControllerDefault() { }

        public SceneControllerDefault(IConfig config) =>
            Configure(config);


        public void Configure(IConfig config)
        {           
            m_Config = (SceneControllerConfig)config;



            IsConfigured = true;
        }

        public override void Init()
        {
            if(IsConfigured == false)
            {
                Send("Instance was not configured. Initialization was failed!", LogFormat.Worning);
                return;
            }
            
            
            IsInitialized = true;
        }

        public override void Dispose()
        {


            IsInitialized = false;
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
            scene = null;
            if (CacheHandler.Contains<TScene>())
            {
                scene = m_CacheHandler.Get();
                return true;
            }

            return false;
        }


    }

    public struct SceneControllerConfig : IConfig
    {
        public Cache<IScene> Cache { get; }

        public SceneControllerConfig(Cache<IScene> cache)
        {
            Cache = cache;
        }
    }

    public interface ISceneController : IController, IConfigurable
    {
        Task Activate<TScene>()
            where TScene : SceneObject, IScene;

    }
}