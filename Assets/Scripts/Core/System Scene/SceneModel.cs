using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;
using Core.Factory;


namespace Core.Scene
{
    [Serializable]
    public abstract class SceneModel : ModelBasic, IScene
    {
        [Header("Debug")]
        [SerializeField] bool m_Debug = true;

        [Header("Stats")]
        [SerializeField] bool m_Configured;
        [SerializeField] bool m_Initialized;
        [SerializeField] bool m_Loaded;
        [SerializeField] bool m_Activated;

        private SceneConfig m_Config;

        //private static List<IScene> ScenesRegistered = new List<IScene>(10);




        private static List<IScene> m_Scenes;

        public int Index { get; private set; }

        public event Action<bool> Configured;
        public event Action<bool> Initialized;
        public event Action<bool> Loaded;
        public event Action<bool> Activated;
        public event Action<ILoadable> LoadRequired;

        public enum Params
        {
            Config,
            Factory
        }

        // CONFIGURE //
        public override void Configure(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
            {
                try { m_Config = (SceneConfig)args[config]; }
                catch { Debug.LogWarning("Scene config was not found. Configuration failed!"); }
                return;
            }


            m_Config = (SceneConfig)args[config];
            Index = m_Config.Index;

            Configured?.Invoke(m_Configured = true);
            if (m_Debug) Debug.Log($"{this.GetName()} configured.");
        }

        public override void Init()
        {
            //ScenesRegistered.Add(m_Scene);



            Initialized?.Invoke(m_Initialized = true);
            if (m_Debug) Debug.Log($"{this.GetName()} initialized.");

        }

        public override void Dispose()
        {


            Initialized?.Invoke(m_Initialized = false);
            if (m_Debug) Debug.Log($"{this.GetName()} disposed.");
        }


        // LOAD //
        public virtual void Load()
        {

            Loaded?.Invoke(m_Loaded = true);
            if (m_Debug) Debug.Log($"{this.GetName()} loaded.");
        }

        public virtual void Unload()
        {
            Loaded?.Invoke(m_Loaded = false);
            if (m_Debug) Debug.Log($"{this.GetName()} unloaded.");
        }


        // ACTIVATE //
        public virtual void Activate()
        {
            Activated?.Invoke(m_Activated = true);
            if (m_Debug) Debug.Log($"{this.GetName()} activated.");
        }

        public virtual void Deactivate()
        {
            Activated?.Invoke(m_Activated = false);
            if (m_Debug) Debug.Log($"{this.GetName()} deactivated.");
        }



        protected virtual IEnumerator LoadAsync()
        {
            UScene uScene;

            var sceneNumber = SceneManager.sceneCount;
            for (int i = 0; i < sceneNumber; i++)
            {
                uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == Index)
                    yield return null;

            }

            try
            {
                var loading = SceneManager.LoadSceneAsync(Index, LoadSceneMode.Additive);
                var loadingTime = 10f;
                while (loadingTime > 0 && loading.progress < 1f)
                {
                    if (loadingTime <= 0f)
                        Debug.LogWarning($"Can't loading scene by index {Index}. Load time is up!");

                    loadingTime -= Time.deltaTime;
                }

            }
            catch
            {
                Debug.LogWarning($"Can't loading scene by index {Index}. Scene is not found.");
            }
        }

        protected virtual IEnumerator ActivateAsync()
        {
            UScene uScene = SceneManager.GetActiveScene();
            if (uScene.buildIndex == Index)
                yield return null;

            var sceneNumber = SceneManager.sceneCount;
            for (int i = 0; i < sceneNumber; i++)
            {
                uScene = SceneManager.GetSceneAt(i);
                if (uScene.buildIndex == Index)
                {
                    SceneManager.SetActiveScene(uScene);
                    yield return null;
                }
            }

            Debug.LogWarning($"Can't activate scene by index {Index}. Scene is not loaded!.");
        }

        protected virtual IEnumerator DeactivateAsync()
        {

            Debug.LogWarning($"Can't deactivate scene by index {Index}. Scene is not found.");
            yield return null;
        }

        protected virtual IEnumerator UnloadAsync()
        {

            Debug.LogWarning($"Can't unload scene by index {Index}. Scene is not found.");
            yield return null;
        }



        // FACTORY //
        public static TScene Get<TScene>(params object[] args)
        where TScene : IScene
        {
            IFactory factoryCustom = null;

            if (args.Length > 0)
            {
                try { factoryCustom = (IFactory)args[(int)Params.Factory]; }
                catch { Debug.Log("Custom factory not found! The instance will be created by default."); }
            }

            var factory = (factoryCustom != null) ? factoryCustom : new FactoryDefault();
            var instance = factory.Get<TScene>(args);

            return instance;
        }

        // CONFIG //
        public class SceneConfig : IConfig
        {
            public int Index { get; private set; }

            public SceneConfig(int index)
            {
                Index = index;
            }

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
}

namespace Core
{
    public interface IScene : IComponent, IConfigurable, ILoadable
    {
        int Index { get; }

    }



}
