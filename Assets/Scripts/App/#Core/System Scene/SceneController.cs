using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;
using Core.Signal;
using Core.Factory;


namespace App.Scene
{
    [Serializable]
    public class SceneController : ModelConfigurable, ISceneController
    {

        private bool m_isDebug = true;
        private string m_DebugLabel = "SceneController";

        private SceneControllerConfig m_Config;
        //private IScene m_Scene;
        //private ISignal m_SignalSceneActivate;



        private IFactory m_SceneFactory;
        private List<IScene> m_Scenes;

        private IScene m_SceneActive;

        //private SceneProvider m_SceneProvider;


        private Dictionary<Type, SceneIndex> m_SceneIndexMap = new Dictionary<Type, SceneIndex>(10);


        public event Action<IResult> SceneLoaded;
        public event Action<IResult> SceneActivated;



        public SceneController() { }
        public SceneController(params object[] args)
            => Init(args);


        // SUBSCRIBE //
        public virtual void Subscribe()
        {
            //SignalProvider.SignalCalled += OnSignalCalled;
        }

        public virtual void Unsubscribe()
        {
            //SignalProvider.SignalCalled -= OnSignalCalled;
        }




        // CONFIGURE //

        public override void Init(params object[] args)
        {

            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (SceneControllerConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {m_DebugLabel} config was not found. Configuration failed!"); return; }


            m_SceneFactory = m_Config.SceneFactory;

            m_Scenes = new List<IScene>();

            //SetIndex<SceneCore>(SceneIndex.Core);
            //SetIndex<SceneMenu>(SceneIndex.Menu);
            //SetIndex<SceneLevel>(SceneIndex.Level);
            //SetIndex<SceneTotals>(SceneIndex.Totals);


            //m_SceneProvider = new SceneProvider();


            foreach (var scene in m_Scenes)
            {
                //scene.LoadRequired += OnSceneLoadRequired;
                scene.Loaded += OnSceneLoaded;
                scene.Activated += OnSceneActivated;
            }


            base.Init();
        }

        public override void Dispose()
        {
            foreach (var scene in m_Scenes)
            {
                SceneDeactivate(scene);
                SceneUnload(scene);


                scene.Dispose();
                //scene.LoadRequired += OnSceneLoadRequired;
                scene.Loaded -= OnSceneLoaded;
                scene.Activated -= OnSceneActivated;
            }

            base.Dispose();
        }


        // LOAD //
        private void SceneLoad(IScene scene)
        {

            if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
            if (scene.isLoaded) { $"{scene.GetName()} is already loaded!".Send(this, m_isDebug, LogFormat.Warning); return; }

            //if (scene == null) { return new Result(this, false, $"{scene.GetName()} not found!", m_isDebug, LogFormat.Warning); }
            //if (scene.isLoaded) { return new Result(this, true, $"{scene.GetName()} is already loaded!", m_isDebug, LogFormat.Warning); }

            scene.Load();
        }

        private void SceneUnload(IScene scene)
        {
            if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
            if (!scene.isLoaded) { $"{scene.GetName()} is already unloaded!".Send(this, m_isDebug, LogFormat.Warning); return; }

            scene.Unload();
        }

        // ACTIVATE //
        private void SceneActivate(IScene scene)
        {
            if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
            if (!scene.isLoaded) { $"{scene.GetName()} is not loaded!".Send(this, m_isDebug, LogFormat.Warning); return; }
            if (scene.isActivated) { $"{scene.GetName()} is already activated!".Send(this, m_isDebug, LogFormat.Warning); return; }

            scene.Activate();

        }

        private void SceneDeactivate(IScene scene)
        {

            if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
            if (!scene.isLoaded) { $"{scene.GetName()} is not loaded!".Send(this, m_isDebug, LogFormat.Warning); return; }
            if (!scene.isActivated) { $"{scene.GetName()} is already deactivated!".Send(this, m_isDebug, LogFormat.Warning); return; }

            scene.Deactivate();
        }




        public async Task Load<TScene>()
            where TScene : Component, IScene
        {
            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TScene))
            {
                Debug.Log($"{typeof(TScene)} is already loaded");
                return;
            }

            UScene uScene;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                uScene = SceneManager.GetSceneAt(i);
                if (uScene.TryGetComponentOnRootGameObject<TScene>(out var scene))
                {
                    if (scene.GetType() == typeof(TScene))
                    {
                        Debug.LogWarning($"{typeof(TScene)} is already loaded");
                        return;
                    }
                }
            }

            if (GetIndex<TScene>(out var index) == false)
            {
                Debug.LogWarning($"Index of {typeof(TScene)} not found!");
                return;
            }

            SceneManager.LoadSceneAsync((int)index, LoadSceneMode.Additive);
            await Task.Delay(5);
        }

        public async Task Unload<TSceneNext>()
        {

            if (m_SceneActive != null && m_SceneActive.GetType() == typeof(TSceneNext))
            {
                Debug.Log($"{typeof(TSceneNext)} is already loaded!");
                return;
            }

            await Unload();

        }

        public async Task Unload()
        {
            if (m_SceneActive == null)
                return;


            var tSceneActive = m_SceneActive.GetType();

            if (GetIndex(out var index, tSceneActive))
            {
                //UScene uSceneActive = SceneManager.GetActiveScene();
                //UScene uScene = SceneManager.GetSceneByBuildIndex((int)SceneIndex.Core);
                //if (uSceneActive != uScene)
                //    SceneManager.SetActiveScene(uScene);

                SceneManager.UnloadSceneAsync((int)index);
                await Task.Delay(5);

            }
            else
            {
                Debug.LogWarning($"Index of {tSceneActive} not found!");
                return;
            }
        }

        public async Task Activate<TView>()
            where TView : IView
        {
            //await Activate<TView, ViewLoading>();
        }

        public async Task Activate<TScene, TView>()
            where TScene : IScene
            where TView : IView
        {
            //var delay = 5;

            //await TaskHandler.Run(() =>
            //    GetSceneByType<TScene>(out var info), delay, $"Activating scene {typeof(TScene)}.");

            var result = new Result();
            if (result.State)
            {
                if (GetIndex<TScene>(out var index))
                {
                    UScene uSceneActive = SceneManager.GetActiveScene();
                    UScene uScene = SceneManager.GetSceneByBuildIndex((int)index);
                    if (uSceneActive != uScene)
                        SceneManager.SetActiveScene(uScene);
                }

                m_SceneActive = result.Context.Convert<TScene>();
                await m_SceneActive.Enter<TView>();

                SceneActivated?.Invoke(result);
            }
        }

        private IScene SetIndex(SceneIndex index, IScene scene)
        {
            m_SceneIndexMap.Add(scene.GetType(), index);
            return scene;
        }


        private bool GetSceneByType<TScene>(out TScene scene)
        where TScene : IScene
        {

            if (m_SceneProvider.Get<TScene>(out var cacheable))
            {
                scene = (TScene)cacheable;
                return true;
            }

            scene = default(TScene);
            return false;
        }

        private bool GetIndex<TScene>(out SceneIndex index)
        where TScene : IScene
        {
            if (m_SceneIndexMap.TryGetValue(typeof(TScene), out index))
                return true;

            return false;
        }


        private void OnSceneLoaded(IResult result)
        {
            if (result == null)
                return;

            var status = result.State;
            var scene = result.Context is IScene ? (IScene)result.Context : null;

            $"{scene.GetName()} load status changed to {result}!".Send(this, m_isDebug);

            switch (status)
            {
                case true:
                    //Load
                    break;

                case false:
                    //Unload
                    break;

            }

        }

        private void OnSceneActivated(IResult result)
        {
            if (result == null)
                return;

            var status = result.State;
            var scene = result.Context is IScene ? (IScene)result.Context : null;

            $"{scene.GetName()} activation status changed to {result}!".Send(this, m_isDebug);


            switch (status)
            {
                case true:
                    //m_SceneActive = scene;
                    //Load
                    break;

                case false:
                    //Unload
                    break;

            }
        }

        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Login()
        {
            throw new NotImplementedException();
        }

        public void Menu()
        {
            throw new NotImplementedException();
        }

        public void Level()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }





        /*

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
   */
    }
}


namespace Core
{

    public interface ISceneController : IController
    {
        event Action<IResult> SceneLoaded;
        event Action<IResult> SceneActivated;

        void Enter();

        void Login();

        void Menu();

        void Level();

        void Exit();


    }

    public struct SceneControllerConfig : IConfig
    {
        public SceneControllerConfig(IFactory sceneFactory)
        {
            SceneFactory = sceneFactory;
        }

        public IFactory SceneFactory { get; private set; }
    }
}