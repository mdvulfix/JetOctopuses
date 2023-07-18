using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;
using Core.Factory;
using Core.Async;
using System.Threading.Tasks;

namespace Core.Scene
{

    [Serializable]
    public abstract class SceneModel : ModelLoadable
    {
        private bool m_isDebug = true;
        private SceneConfig m_Config;


        [SerializeField] private SceneIndex m_Index;

        private List<IView> m_Views;


        public string Label => "Awaiter";
        public SceneIndex Index => m_Index;


        public event Action<ILoadable> LoadRequired;


        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (SceneConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {Label} config was not found. Configuration failed!"); return; }

            m_Index = m_Config.Index;

        }

        public override void Dispose()
        {

        }





        // ACTIVATE //
        public override void Activate()
        {
            var result = AsyncOperation(() => AwaitActivating());
            var log = result.Log;

            if (!result.State) { log.Send(this, m_isDebug, LogFormat.Warning); return; }

            base.Activate();
        }

        public override void Deactivate()
        {
            var result = AsyncOperation(() => AwaitDeactivating());
            var log = result.Log;

            if (!result.State) { log.Send(this, m_isDebug, LogFormat.Warning); return; }

            base.Deactivate();
        }



        public Task Enter<TView>() where TView : IView
        {
            return null;
        }


        protected virtual bool AwaitLoading()
        {
            UScene uScene;

            var buildIndex = (int)Index;
            var sceneNumber = SceneManager.sceneCount;
            for (int i = 0; i < sceneNumber; i++)
            {
                uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == buildIndex)
                {
                    $"Scene by index {buildIndex} is already loaded.".Send(this, m_isDebug);
                    return true;
                }
            }

            var loading = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            if (loading.progress < 0.9f)
            {
                $"Awaiting loading scene by index {buildIndex}...".Send(this, m_isDebug);
                return false;
            }

            $"Scene by index {buildIndex} successfully loaded.".Send(this, m_isDebug);
            return true;
        }

        protected virtual bool AwaitUnloading()
        {
            var buildIndex = (int)Index;

            Debug.LogWarning($"Can't unload scene by index {buildIndex}. Scene is not found.");
            return false;
        }

        protected virtual bool AwaitActivating()
        {
            UScene uScene = SceneManager.GetActiveScene();

            var buildIndex = (int)Index;
            if (uScene.buildIndex == buildIndex)
            {
                $"Scene by index {buildIndex} is already activated.".Send(this, m_isDebug);
                return true;
            }

            var sceneNumber = SceneManager.sceneCount;
            for (int i = 0; i < sceneNumber; i++)
            {
                uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == buildIndex)
                {
                    SceneManager.SetActiveScene(uScene);
                    $"Scene by index {buildIndex} successfully activated.".Send(this, m_isDebug);
                    return true;
                }
            }

            $"Can't activate scene by index {buildIndex}.".Send(this, m_isDebug, LogFormat.Warning);
            return false;
        }

        protected virtual bool AwaitDeactivating()
        {
            var buildIndex = (int)Index;


            Debug.LogWarning($"Can't deactivate scene by index {buildIndex}. Scene is not found.");
            return false;
        }




        private IResult AsyncOperation(Func<bool> func)
        {
            /*
            using (var awaiter = AwaiterDefault.Get(new AwaiterConfig()))
            {
                awaiter.Init();
                awaiter.Activate();
                return m_Awaiter.Run(this, func);
            }
            */
            return null;

        }


        // FACTORY //
        public static TScene Get<TScene>(IFactory factory, params object[] args)
        where TScene : IScene
        {
            factory = (factory != null) ? factory : new SceneFactory();
            return factory.Get<TScene>(args);
        }



        public struct SceneActionInfo : IActionInfo
        {
            public IScene Scene { get; private set; }

            public SceneActionInfo(IScene scene)
            {
                Scene = scene;
            }

        }

        /*
                // CONFIGURE //
                public override void Configure(params object[] param)
                {
                    Send("Start configuration...");

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
                                //m_Config = (SceneConfig)obj;

                                //Label = m_Config.Label;
                                //Scene = m_Config.Scene;
                                //Index = SceneIndex<TScene>.SetIndex(m_Config.SceneIndex);

                                //m_Screens = m_Config.Screens;
                                //m_ScreenLoading = m_Config.ScreenLoading;
                                //m_ScreenDefault = m_Config.ScreenDefault;

                                Send($"{obj.GetName()} setup.");
                            }
                        }
                    }
                    else
                    {
                        Send("Params are empty. Config setup aborted!", LogFormat.Warning);
                    }


                    //m_CacheHandler = new CacheHandler<IScene>();
                    //m_ScreenController = new ScreenControllerDefault();

                    //IsConfigured = true;
                    //Configured?.Invoke();

                    Send("Configuration completed!");
                }

                public override void Init()
                {

                    Send("Start initialization...");

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

                    m_CacheHandler.Configure(new CacheHandlerConfig(this));
                    m_CacheHandler.Init();
                    RecordToCache();


                    //m_ScreenController.Configure(new ScreenControllerConfig(m_Screens, m_ScreenLoading, m_ScreenDefault));
                    //m_ScreenController.Init();




                    //IsInitialized = true;
                    //Initialized?.Invoke();
                    Send("Initialization completed!");
                }

                public override void Dispose()
                {

                    Send("Start disposing...");

                    foreach (var screen in m_Screens)
                        screen.Dispose();

                    m_ScreenController.Dispose();
                    m_CacheHandler.Dispose();

                    DeleteFromCache();
                    Unsubscribe();

                    //IsInitialized = false;
                    //Disposed?.Invoke();
                    Send("Dispose completed!");
                }

                public virtual void Subscribe()
                {
                    m_CacheHandler.Message += OnMessage;
                    m_ScreenController.Message += OnMessage;

                    foreach (var screen in m_Screens)
                        screen.Message += OnMessage;

                }

                public virtual void Unsubscribe()
                {
                    m_CacheHandler.Message -= OnMessage;
                    m_ScreenController.Message -= OnMessage;

                    foreach (var screen in m_Screens)
                        screen.Message += OnMessage;
                }



                // SCENE //
                public async Task<ITaskResult> Load()
                {
                    if (IsLoaded == true)
                        return new TaskResult(true, Send("The instance was already loaded. The current loading has been aborted!", LogFormat.Warning));

                    //var uSceneLoadingTaskResult = await USceneHandler.USceneLoad(Index);
                    //if (uSceneLoadingTaskResult.Status == false)
                    //    return new TaskResult(false, uSceneLoadingTaskResult.Message);


                    //var uSceneActivateTaskResult = await USceneHandler.USceneActivate(Index);
                    //if (uSceneActivateTaskResult.Status == false)
                    //     return new TaskResult(false, uSceneActivateTaskResult.Message);

                    // Loading scene objects  ...
                    await TaskHandler.Run(() => AwaitSceneLoading(), "Waiting for screen loading...");

                    // Loading screens...
                    foreach (var screen in m_Screens)
                    {
                        var screenLoadTaskResult = await m_ScreenController.ScreenLoad(screen);
                        if (screenLoadTaskResult.Status == false)
                            return new TaskResult(false, screenLoadTaskResult.Message);
                    }

                    //IsLoaded = true;
                    //Loaded?.Invoke();
                    return new TaskResult(true, Send("The instance was loaded."));
                }

                public async Task<ITaskResult> Activate(bool animate = true)
                {
                    if (IsActivated == true)
                        return new TaskResult(true, Send("The scene was already activated. The current activation has been aborted!", LogFormat.Warning));

                    //var uSceneActivateTaskResult = await USceneHandler.USceneActivate(Index);
                    //if (uSceneActivateTaskResult.Status == false)
                    //    return new TaskResult(false, uSceneActivateTaskResult.Message);

                    // Activate  UScene...
                    //var sceneActivate = true;
                    //await TaskHandler.Run(() => AwaitSceneActivation(sceneActivate), "Waiting for screen activation...");

                    var screenLoadTaskResult = await m_ScreenController.ScreenActivate(m_ScreenDefault, animate);
                    if (screenLoadTaskResult.Status == false)
                        return new TaskResult(false, screenLoadTaskResult.Message);

                    //IsActivated = true;
                    //Activated?.Invoke();
                    return new TaskResult(true, Send("The instance was activated."));
                }

                public async Task<ITaskResult> Deactivate()
                {
                    if (IsActivated != true)
                        return new TaskResult(true, Send("The scene was already deactivated. The current deactivation has been aborted!", LogFormat.Warning));

                    foreach (var screen in m_Screens)
                    {
                        var screenDeactivateTaskResult = await m_ScreenController.ScreenDeactivate(screen);
                        if (screenDeactivateTaskResult.Status == false)
                            return new TaskResult(false, screenDeactivateTaskResult.Message);
                    }

                    // Activate  UScene...
                    var sceneActivate = false;
                    await TaskHandler.Run(() => AwaitSceneActivation(sceneActivate), "Waiting for screen deactivation...");

                    //IsActivated = false;
                    return new TaskResult(true, Send("The instance was deactivated."));
                }

                public async Task<ITaskResult> Unload()
                {
                    if (IsLoaded == false)
                        return new TaskResult(true, Send("The instance was already unloaded. The current unloading has been aborted!", LogFormat.Warning));


                    // Loading screens...
                    foreach (var screen in m_Screens)
                    {
                        var screenLoadTaskResult = await m_ScreenController.ScreenUnload(screen);
                        if (screenLoadTaskResult.Status == false)
                            return new TaskResult(false, screenLoadTaskResult.Message);
                    }


                    // Loading scene objects  ...
                    await TaskHandler.Run(() => AwaitSceneUnloading(), "Waiting for scene unloading...");

                    //var sceneCoreIndex = SceneIndex<SceneCore>.Index;
                    //var uSceneActivateTaskResult = await USceneHandler.USceneActivate(sceneCoreIndex);
                    //if (uSceneActivateTaskResult.Status == false)
                    //    return new TaskResult(false, uSceneActivateTaskResult.Message);


                    //var uSceneLoadingTaskResult = await USceneHandler.USceneLoad(Index);
                    //if (uSceneLoadingTaskResult.Status == false)
                    //    return new TaskResult(false, uSceneLoadingTaskResult.Message);

                    //IsLoaded = true;
                    //Loaded?.Invoke();
                    return new TaskResult(true, Send("The instance was loaded."));
                }


                // SCREEN //
                public async Task<ITaskResult> ScreenLoad(IScreen screen) =>
                    await m_ScreenController.ScreenLoad(screen);

                public async Task<ITaskResult> ScreenActivate(IScreen screen, bool animate = true) =>
                    await m_ScreenController.ScreenActivate(screen, animate);

                public async Task<ITaskResult> ScreenDeactivate(IScreen screen) =>
                    await m_ScreenController.ScreenDeactivate(screen);

                public async Task<ITaskResult> ScreenUnload(IScreen screen) =>
                    await m_ScreenController.ScreenUnload(screen);

                // AWAIT //
                private bool AwaitSceneLoading()
                {
                    //if (SceneObject != null)
                    //    return true;
                    //
                    // var obj = GameObjectHandler.CreateGameObject(Label);
                    //SceneObject = GameObjectHandler.SetComponent<SceneObject>(obj);

                    return true;
                }

                private bool AwaitSceneUnloading()
                {
                    //if (SceneObject == null)
                    //    return true;

                    //var obj = SceneObject.gameObject;
                    //GameObjectHandler.DestroyGameObject(obj);
                    return true;
                }

                private bool AwaitSceneActivation(bool activate)
                {
                    //if (SceneObject == null)
                    //    return false;


                    // var obj = SceneObject.gameObject;
                    //obj.SetActive(activate);
                    return true;
                }


                // CACHE //
                private void RecordToCache() =>
                    RecordRequired?.Invoke();

                private void DeleteFromCache() =>
                    DeleteRequired?.Invoke();


        */
    }


    public partial class SceneFactory : Factory<IScene>
    {
        public SceneFactory()
        {
            Set<SceneLogin>(Constructor.Get((args) => GetSceneLogin(args)));
            Set<SceneMenu>(Constructor.Get((args) => GetSceneMenu(args)));
            Set<SceneLevel>(Constructor.Get((args) => GetSceneLevel(args)));
        }
    }


}

namespace Core
{

    public enum SceneIndex
    {
        None,
        Login,
        Menu,
        Level
    }


    public interface IScene : IConfigurable, ILoadable, IActivable
    {
        SceneIndex Index { get; }

        Task Enter<TView>()
            where TView : IView;


    }

    public struct SceneConfig : IConfig
    {
        public SceneIndex Index { get; private set; }
        public IView[] Views { get; private set; }

        public SceneConfig(SceneIndex index, IView[] views)
        {
            Index = index;
            Views = views;
        }
    }


    public enum SceneState
    {
        None,
        ScreenIn,
        ScreenEnterSuccess
    }

    public enum SceneAction
    {
        None,
        LevelRun,
        LevelPause
    }

    public enum LevelIndex
    {
        None,
        Level1,
        Level2

    }







}
