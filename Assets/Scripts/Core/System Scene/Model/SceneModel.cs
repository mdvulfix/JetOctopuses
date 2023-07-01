using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;
using Core.Factory;
using Core.Async;

namespace Core.Scene
{

   [Serializable]
   public abstract class SceneModel : ModelBasic
   {

      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;
      [SerializeField] private bool m_isLoaded;
      [SerializeField] private bool m_isActivated;


      [SerializeField] private SceneIndex m_Index;

      private List<IScreen> m_Screens;

      private IAwaiter m_Awaiter;



      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected SceneConfig m_Config;




      public SceneIndex Index => m_Index;
      public bool isLoaded => m_isLoaded;
      public bool isActivated => m_isActivated;

      public event Action<IResult> Configured;
      public event Action<IResult> Initialized;
      public event Action<IResult> Loaded;
      public event Action<IResult> Activated;
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

         var result = default(IResult);
         var log = "...";

         if (args.Length > 0)
         {
            try { m_Config = (SceneConfig)args[config]; }
            catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
         }

         m_Index = m_Config.Index;



         m_isConfigured = true;
         log = $"{this.GetName()} configured.";
         result = new Result(this, m_isConfigured, log, m_isDebug);
         Configured?.Invoke(result);
      }

      public override void Init()
      {
         var result = default(IResult);
         var log = "...";

         var awaiterConfig = new AwaiterConfig();
         m_Awaiter = AwaiterDefault.Get();
         m_Awaiter.Configure();
         m_Awaiter.Init();
         m_Awaiter.Activate();




         m_isInitialized = true;
         log = $"{this.GetName()} initialized.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }

      public override void Dispose()
      {
         var result = default(IResult);
         var log = "...";

         m_Awaiter.Deactivate();
         m_Awaiter.Dispose();


         m_isInitialized = false;
         log = $"{this.GetName()} disposed.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }



      // LOAD //
      public virtual void Load()
      {

         var result = AsyncOperation(() => AwaitLoading());
         var log = result.Log;

         if (!result.Status) { log.Send(this, m_isDebug, LogFormat.Warning); return; }


         m_isLoaded = true;
         //log = $"{this.GetName()} loaded.".Send(this, m_isDebug);
         result = new Result(this, m_isLoaded, log, m_isDebug);
         Loaded?.Invoke(result);

      }

      public virtual void Unload()
      {
         var result = AsyncOperation(() => AwaitUnloading());
         var log = result.Log;

         if (!result.Status) { log.Send(this, m_isDebug, LogFormat.Warning); return; }

         m_isLoaded = false;
         //log = $"{this.GetName()} unloaded.";
         result = new Result(this, m_isLoaded, log, m_isDebug);
         Loaded?.Invoke(result);

      }


      // ACTIVATE //
      public virtual void Activate()
      {
         var result = AsyncOperation(() => AwaitActivating());
         var log = result.Log;

         if (!result.Status) { log.Send(this, m_isDebug, LogFormat.Warning); return; }

         m_isActivated = true;
         //log = $"{this.GetName()} activated."
         result = new Result(this, m_isActivated, log, m_isDebug);
         Activated?.Invoke(result);

      }

      public virtual void Deactivate()
      {
         var result = AsyncOperation(() => AwaitDeactivating());
         var log = result.Log;

         if (!result.Status) { log.Send(this, m_isDebug, LogFormat.Warning); return; }


         m_isActivated = false;
         //log = $"{this.GetName()} deactivated."
         result = new Result(this, m_isActivated, log, m_isDebug);
         Activated?.Invoke(result);

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
         using (var awaiter = AwaiterDefault.Get(new AwaiterConfig()))
         {
            awaiter.Init();
            awaiter.Activate();
            return m_Awaiter.Run(this, func);
         }

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

   public enum SceneIndex
   {
      None,
      Login,
      Menu,
      Level
   }


   public interface IScene : IComponent, IConfigurable, ILoadable, IActivable
   {
      SceneIndex Index { get; }
   }

   public class SceneConfig : IConfig
   {
      public SceneIndex Index { get; private set; }

      public SceneConfig(SceneIndex index)
      {
         Index = index;
      }

   }

}
