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
        private Register<IScene> m_SceneRegister;

        private IScene m_SceneActive;

        public bool IsConfigured { get; private set; }

        public Action<IScene> SceneActivated;

        public SceneControllerDefault() { }

        public SceneControllerDefault(IConfig config) =>
            Configure(config);


        public void Configure(IConfig config)
        {

            m_Config = (SceneControllerConfig)config;

            m_SceneRegister = new Register<IScene>();
            foreach (var scene in m_Config.Scenes)
            {
                m_SceneRegister.Set(scene);
            }

            IsConfigured = true;

        }

        public override void Init()
        {
            if(IsConfigured == false)
            {
                Send("Instance was not configured. Initialization was failed!", true);
                return;
            }
            
            
            IsInitialized = true;
        }

        public override void Dispose()
        {


            IsInitialized = false;
        }

        public async Task Activate<TScene>() where TScene : UComponent, IScene
        {
            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
            {
                Send($"{typeof(TScene).Name} is already active!");
                return;
            }

            await SceneActivate<TScene>();

        }

        /*
        private async Task SceneActivate<TScene>() where TScene : UComponent, IScene
        {
            if (GetSceneIndex<TScene>(out var index))
                await SceneActivate<TScene>(index);
            else
                Send($"{typeof(TScene).Name} not set to scene indexes!", true);

        }
        */

        private async Task SceneActivate<TScene>() where TScene : UComponent, IScene
        {

            var sceneIndex = SceneIndex<TScene>.Index;
            await USceneHandler.Activate(sceneIndex);

            TScene scene = null;
            await TaskHandler.Run(() => AwaitSceneActivation<TScene>(sceneIndex, out scene), "Waiting for scene activation...");

            if (scene == null)
            {
                Send($"{scene.GetType().Name} not found!", true);
                return;
            }

            m_SceneActive = scene;
            m_SceneActive.Activate<ScreenLoading>();

        }

        private bool AwaitSceneActivation<TScene>(SceneIndex? index, out TScene scene) where TScene : UComponent, IScene
        {
            scene = null;
            if (RegisterHandler.Contains<TScene>())
            {
                scene = RegisterHandler.Get<TScene>();
                return true;
            }

            return false;
        }


        /*
private bool GetSceneIndex<TScene>(out SceneIndex index)
where TScene : IScene
{
   index = SceneIndex<TScene>.Index;
   if(index != default(SceneIndex))
       return true;

   return false;
}
*/
    }

    public class SceneControllerConfig : Config
    {
        public IScene[] Scenes { get; private set; }

        public SceneControllerConfig(Instance info, IScene[] scenes): base(info)
        {
            Scenes = scenes;
        }

    }

    public interface ISceneController : IController, IConfigurable
    {
        Task Activate<TScene>()
            where TScene : UComponent, IScene;

    }
}