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

        private Dictionary<Type, SceneIndex?> m_SceneIndexes;

        public bool IsInitialized {get; private set;}
        public Action<IScene> SceneActivated;

        public SceneControllerDefault()
        {
            m_SceneIndexes = new Dictionary<Type, SceneIndex?>(5);
        }

        public override void Init()
        {           
            SetSceneIndex<SceneLogin>(SceneLogin.Index);
            SetSceneIndex<SceneMenu>(SceneMenu.Index);
            SetSceneIndex<SceneLevel>(SceneLevel.Index);

        }

        public override void Dispose()
        {
 
        }

        public async Task Activate<TScene>() where TScene : UComponent, IScene
        {
            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
                return;

            await SceneActivate<TScene>();
        }

        private async Task SceneActivate<TScene>() where TScene : UComponent, IScene
        {
            if (GetSceneIndex<TScene>(out var index))
                await SceneActivate<TScene>(index);

        }

        private async Task SceneActivate<TScene>(SceneIndex? sceneIndex) where TScene: UComponent, IScene
        {
            await USceneHandler.SceneActivate(sceneIndex);

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
            if (RegisterHandler.Get<TScene>(out scene))
                return true;

            return false;
        }

        private void SetSceneIndex<TScene>(SceneIndex index) =>
            m_SceneIndexes.Add(typeof(TScene), index);

        private bool GetSceneIndex<TScene>(out SceneIndex? index)
        where TScene : IScene
        {
            index = null;
            if (m_SceneIndexes.ContainsKey(typeof(TScene)))
            {
                m_SceneIndexes.TryGetValue(typeof(TScene), out index);
                return true;
            }

            return false;
        }

    }


    public interface ISceneController: IController
    {
        Task Activate<TScene>() 
            where TScene : UComponent, IScene;
        
    }




}