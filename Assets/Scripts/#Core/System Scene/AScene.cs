using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;
using Core.Cache;
using Core.Factory;

/*
namespace Core.Scene
{

   [Serializable]
   public abstract class AScene : AModel //, ICacheable , ISubscriber
   {
       private static List<IScene> ScenesRegistered = new List<IScene>(10);


       [SerializeField] bool m_Debug;

       [SerializeField] bool m_IsConfigured;
       [SerializeField] bool m_IsInitialized;
       [SerializeField] bool m_IsLoaded;
       [SerializeField] bool m_IsActivated;




       private SceneConfig m_Config;
       private IScene m_Scene;

       private IScreen[] m_Screens;

       protected enum ConfigParams
       {
           Config,
           Factory
       }

       public event Action<IScene> LoadRequired;
       public event Action<IScene, bool> Loaded;
       public event Action<IScene, bool> Activated;

       // CONFIGURE //
       public override void Configure(params object[] args)
       {
           m_Debug = true;

           if (args.Length > 0)
               foreach (var arg in args)
                   m_IsConfigured = arg is IConfig ? Setup(arg as IConfig) : false;



           if (m_Debug)
               if (m_IsConfigured)
                   Debug.Log($"{m_Scene.GetName()} configured successfully.");
       }

       public override void Init()
       {
           m_IsInitialized = true;

           if (m_Debug)
               if (m_IsInitialized)
                   Debug.Log($"{m_Scene.GetName()}  initialized successfully.");


           Loaded += OnLoaded;
           Activated += OnActivated;
           ScenesRegistered.Add(m_Scene);

       }

       public override void Dispose()
       {
           m_IsInitialized = false;



           if (m_Debug)
               if (!m_IsInitialized)
                   Debug.Log($"{m_Scene.GetName()}  disposed successfully.");


           ScenesRegistered.Remove(m_Scene);
           Loaded -= OnLoaded;
           Activated -= OnActivated;
       }


       protected virtual bool Setup(IConfig config)
       {
           if (config is SceneConfig)
           {
               m_Config = (SceneConfig)config;
               m_Scene = m_Config.Scene;
               //m_Screens = m_Config.Screens;

               return true;
           }

           if (m_Debug)
               Debug.LogWarning("Scene config not found. Configuration failed!");

           return false;
       }


       public IEnumerator Load()
       {
           UScene uScene;

           var sceneNumber = SceneManager.sceneCount;
           for (int i = 0; i < sceneNumber; i++)
           {
               uScene = SceneManager.GetSceneAt(i);
               if (uScene.buildIndex == m_Scene.BuildIndex)
               {
                   Loaded?.Invoke(m_Scene, true);
                   yield return null;
               }
           }

           try
           {
               var loading = SceneManager.LoadSceneAsync(m_Scene.BuildIndex, LoadSceneMode.Additive);
               var loadingTime = 10f;
               while (loadingTime > 0 && loading.progress < 1f)
               {
                   if (loadingTime <= 0f)
                       Debug.LogWarning($"Can't loading scene by index {m_Scene.BuildIndex}. Load time is up!");

                   loadingTime -= Time.deltaTime;
               }

               Loaded?.Invoke(m_Scene, true);
           }
           catch
           {
               Debug.LogWarning($"Can't loading scene by index {m_Scene.BuildIndex}. Scene is not found.");
           }
       }

       public IEnumerator Activate()
       {
           UScene uScene = SceneManager.GetActiveScene();
           if (uScene.buildIndex == m_Scene.BuildIndex)
               yield return null;


           var sceneNumber = SceneManager.sceneCount;
           for (int i = 0; i < sceneNumber; i++)
           {
               uScene = SceneManager.GetSceneAt(i);
               if (uScene.buildIndex == m_Scene.BuildIndex)
               {
                   SceneManager.SetActiveScene(uScene);
                   Activated?.Invoke(m_Scene, true);
                   yield return null;
               }
               else
               {
                   LoadRequired?.Invoke(m_Scene);
                   Debug.LogWarning($"Can't activate scene by index {m_Scene.BuildIndex}. Scene is not loaded!.");
               }
           }


       }

       public IEnumerator Deactivate()
       {
           m_IsActivated = false;

           Debug.LogWarning($"Can't deactivate scene by index {m_Scene.BuildIndex}. Scene is not found.");
           yield return null;
       }

       public IEnumerator Unload()
       {
           m_IsLoaded = false;

           Debug.LogWarning($"Can't unload scene by index {m_Scene.BuildIndex}. Scene is not found.");
           yield return null;
       }


       private void OnLoaded(IScene scene, bool result)
       {
           m_IsLoaded = result;


           //if (SceneObject != null)
           //    return true;
           //
           // var obj = GameObjectHandler.CreateGameObject(Label);
           //SceneObject = GameObjectHandler.SetComponent<SceneObject>(obj);

       }


       private void OnActivated(IScene scene, bool result)
       {
           foreach (var sceneRegistered in ScenesRegistered)
               sceneRegistered.Deactivate();


           m_IsActivated = result;

           //if (SceneObject != null)
           //    return true;
           //
           // var obj = GameObjectHandler.CreateGameObject(Label);
           //SceneObject = GameObjectHandler.SetComponent<SceneObject>(obj);

       }



       // FACTORY //
       public static TScene Get<TScene>(params object[] args)
       where TScene : IScene
       {
           IFactory factoryCustom = null;

           if (args.Length > 0)
               try { factoryCustom = (IFactory)args[(int)ConfigParams.Factory]; }
               catch { Debug.Log("Custom factory not found! The instance will be created by default."); }


           var factory = (factoryCustom != null) ? factoryCustom : new FactoryDefault();
           var instance = factory.Get<TScene>(args);

           return instance;
       }



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



   }
}




       public event Action<IEventArgs> PlayButtonClicked;
       public event Action<IEventArgs> OptionsButtonClicked;
       public event Action<IEventArgs> ExitButtonClicked;

       protected override void OnAwake()
       {

       }

       protected override void OnEnable()
       {
           m_Play.onClick.AddListener(() => Play());
           m_Options.onClick.AddListener(() => Options());
           m_Exit.onClick.AddListener(() => Exit());

           Add(this);

       }

       protected override void OnDisable()
       {
           m_Play.onClick.RemoveListener(() => Play());
           m_Options.onClick.RemoveListener(() => Options());
           m_Exit.onClick.RemoveListener(() => Exit());

           Remove(this);
       }


       private void Play(IEventArgs args = null)
       {
           PlayButtonClicked?.Invoke(args);
       }

       private void Options(IEventArgs args = null)
       {
           OptionsButtonClicked?.Invoke(args);
       }

       private void Exit(IEventArgs args = null)
       {
           ExitButtonClicked?.Invoke(args);
       }

       */