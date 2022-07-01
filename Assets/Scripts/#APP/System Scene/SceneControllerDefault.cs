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
        private IScene m_SceneActive;

        public bool IsInitialized {get; private set;}
        public Action<IScene> SceneActivated;

        public SceneControllerDefault() { }

        public override void Init()
        {           


        }

        public override void Dispose()
        {
 
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
        
        private async Task SceneActivate<TScene>() where TScene: UComponent, IScene
        {
            
            var sceneIndex = SceneIndex<TScene>.Index;
            await USceneHandler.Activate(sceneIndex);

            TScene scene = null;
            await TaskHandler.Run(() => AwaitSceneActivation<TScene>(sceneIndex, out scene),"Waiting for scene activation...");

            if(scene == null)
            {
                Send($"{scene.GetType().Name} not found!", true);
                return;
            }
            
            m_SceneActive = scene;
            m_SceneActive.Activate<ScreenLoading>();

        }

        private bool AwaitSceneActivation<TScene>(SceneIndex? index, out TScene scene) where TScene: UComponent, IScene
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


    public interface ISceneController: IController
    {
        Task Activate<TScene>() 
            where TScene : UComponent, IScene;
        
    }
}