using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using SERVICE.Handler;
using APP.Screen;

namespace APP.Scene
{
    public class SceneControllerDefault : Controller, ISceneController
    {
        public bool IsInitialized {get; private set;}
        
        private IScene m_SceneActive;
        private IScreen m_ScreenActive;

        private IScreenController m_ScreenController;

        private Dictionary<Type, SceneIndex?> m_SceneIndexes;

        public SceneControllerDefault()
        {
            m_SceneIndexes = new Dictionary<Type, SceneIndex?>(5);
            m_ScreenController = new ScreenControllerDefault();
        }

        public override void Init()
        {           
            SetSceneIndex<SceneCore>(SceneCore.Index);
            SetSceneIndex<SceneMenu>(SceneMenu.Index);
            SetSceneIndex<SceneLevel>(SceneLevel.Index);

            m_ScreenController.Init();
        }

        public override void Dispose()
        {
            m_ScreenController.Dispose();
        }

        public async Task Activate<TScene>()
        where TScene : UComponent, IScene
        {
            await SceneActivate<TScene>();
            //ScreenActivate<ScreenLoading>();

        }

        public async Task Activate<TScene, TScreen>()
        where TScene : UComponent, IScene
        where TScreen : UComponent, IScreen
        {
            await SceneActivate<TScene>();
            await ScreenActivate<TScreen>();
        }


        // SCREEN METHODS
        private async Task ScreenActivate<TScreen>()
        where TScreen : IScreen
        {
            var animate = true;
            await m_ScreenController.Activate<TScreen>(animate);


            //SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            //Send ($"{scene} loaded...");


        }


        // SCENE METHODS
        private async Task SceneActivate<TScene>()
        where TScene : UComponent, IScene
        {

            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
                return;

            if (GetSceneIndex<TScene>(out var index))
            {
                await SceneActivate<TScene>(index);
            }

            //SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            //Send ($"{scene} loaded...");
        }

        private async Task SceneActivate<TScene>(SceneIndex? sceneIndex)
        where TScene: UComponent
        {
            //SceneActivated?.Invoke();
            //Send($"{scene} activated...");

            var index = (int) sceneIndex;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == index)
                {

                    await TaskHandler.Run(() => SceneLoadingAwait<TScene>(sceneIndex));

                    SceneManager.SetActiveScene(uScene);
                    //SceneActivated?.Invoke();
                    return;
                }
            }

            await SceneLoad<TScene>(sceneIndex);
            await SceneActivate<TScene>(sceneIndex);

            Send($"{sceneIndex} activated...");

        }

        private async void SceneLoad<TScene>()
        where TScene : IScene
        {
            if (GetSceneIndex<TScene>(out var index))
            {
                await SceneLoad<TScene>(index);
            }

        }

        private async Task SceneLoad<TScene>(SceneIndex? sceneIndex)
        {

            var operation = SceneManager.LoadSceneAsync((int) sceneIndex, LoadSceneMode.Additive);
            await TaskHandler.Run(() => USceneLoadingAwait(operation));

            //SceneLoaded?.Invoke();
            Send($"{sceneIndex} loaded...");
        }

        private void SceneUnload<TScene>()
        where TScene : IScene
        {

            //SceneManager.UnloadSceneAsync((int)scene);
            //Send ($"{scene} unloaded...");
            //SetActive(SceneIndex.Core);
        }

        private void SceneUnload(SceneIndex scene)
        {

            //SceneManager.UnloadSceneAsync((int)scene);
            //Send ($"{scene} unloaded...");
            //SetActive(SceneIndex.Core);
        }

        private void SceneReload<TScene>()
        where TScene : IScene
        {
            //Unload(scene);
            //Load(scene);
            //Send ($"{scene} reloaded...");
        }

        private void SceneReload(SceneIndex scene)
        {
            //Unload(scene);
            //Load(scene);
            //Send ($"{scene} reloaded...");
        }

        private void SetSceneIndex<TScene>(SceneIndex index)
        {
            m_SceneIndexes.Add(typeof(TScene), index);
        }

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

        private bool USceneLoadingAwait(AsyncOperation operation)
        {
            if (operation.isDone)
                return true;

            return false;
        }

        private bool SceneLoadingAwait<TScene>(SceneIndex? index)
        where TScene: UComponent
        {
            if (RegisterHandler.Get<TScene>(out var instance))
                return true;

            return false;
        }




    }

    public interface ISceneController: IController
    {
        Task Activate<TScene>()
        where TScene : UComponent, IScene;
        
        Task Activate<TScene, TScreen>()
        where TScene : UComponent, IScene
        where TScreen : UComponent ,IScreen;


    }


    public enum SceneIndex
    {
        Service,
        Core,
        Menu,
        Level
    }

}