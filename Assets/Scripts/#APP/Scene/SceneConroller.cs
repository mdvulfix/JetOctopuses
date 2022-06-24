using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using SERVICE.Handler;
using APP.Screen;

namespace APP.Scene
{
    public enum SceneIndex
    {
        Service,
        Core,
        Menu,
        Level
    }
    
    public class SceneControllerDefault : Controller, ISceneController
    {
        private IScene m_SceneActive;
        private IScreen m_ScreenActive;

        private ScreenController m_ScreenController;
        private TaskHandler m_TaskHandler;

        private Dictionary<Type, SceneIndex?> m_SceneIndexes;

        public SceneControllerDefault(IConfig config) =>
            Configure(config);

        public void Configure(IConfig config)
        {
            m_SceneIndexes = new Dictionary<Type, SceneIndex?>(5);

            m_TaskHandler = new TaskHandler();
            
            var sceneControllerConfig = new SceneControllerConfig();
            m_ScreenController = new ScreenController(sceneControllerConfig);

        }

        public override void Init()
        {
            SetSceneIndex<SceneCore>(SceneCore.Index);
            SetSceneIndex<SceneMenu>(SceneMenu.Index);
            SetSceneIndex<SceneLevel>(SceneLevel.Index);

            m_TaskHandler.Init();
            m_ScreenController.Init();
        }

        public override void Dispose()
        {
            m_TaskHandler.Dispose();
            m_ScreenController.Dispose();
        }

        public void Activate<TScene>()
        where TScene : IScene
        {
            SceneActivate<TScene>();
            //ScreenActivate<ScreenLoading>();

        }

        public void Activate<TScene, TScreen>()
        where TScene : IScene
        where TScreen : IScreen
        {
            SceneActivate<TScene>();
            ScreenActivate<TScreen>();
        }


        // SCREEN METHODS
        private bool ScreenActivate<TScreen>()
        where TScreen : IScreen
        {
            var animate = true;
            if (m_ScreenController.Activate<TScreen>(animate))
                return true;

            //SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            //Send ($"{scene} loaded...");

            return false;
        }


        // SCENE METHODS
        private async void SceneActivate<TScene>()
        where TScene : IScene
        {

            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
                return;

            if (GetSceneIndex<TScene>(out var index))
            {
                await SceneActivate(index);
            }

            //SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            //Send ($"{scene} loaded...");
        }

        private async Task SceneActivate(SceneIndex? sceneIndex)
        {
            //SceneActivated?.Invoke();
            //Send($"{scene} activated...");

            var index = (int) sceneIndex;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == index)
                {

                    //m_TaskHandler.Run();

                    SceneManager.SetActiveScene(uScene);
                    //SceneActivated?.Invoke();
                    return;
                }
            }

            await SceneLoad(sceneIndex);
            await SceneActivate(sceneIndex);

            Send($"{sceneIndex} activated...");

        }

        private async void SceneLoad<TScene>()
        where TScene : IScene
        {
            if (GetSceneIndex<TScene>(out var index))
            {
                await SceneLoad(index);
            }

        }

        private async Task SceneLoad(SceneIndex? sceneIndex)
        {
            var operation = SceneManager.LoadSceneAsync((int) sceneIndex, LoadSceneMode.Additive);
            await m_TaskHandler.Run(() => SceneOperationAwait(operation));

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

        private bool SceneOperationAwait(AsyncOperation operation)
        {
            while (true)
            {
                if (operation.isDone)
                    return true;
            }
        }

        private string Send(string text, bool worning = false) =>
            LogHandler.Send(this, true, text, worning);

    }

    public interface ISceneController: IController
    {
        void Activate<TScene>()
        where TScene : IScene;
        
        void Activate<TScene, TScreen>()
        where TScene : IScene
        where TScreen : IScreen;


    }

    public class SceneControllerConfig : IConfig
    {
        public SceneControllerConfig()
        {

        }
    }




}