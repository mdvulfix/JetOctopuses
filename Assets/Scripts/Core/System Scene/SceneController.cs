using System;
using System.Threading.Tasks;
using UnityEngine;
using UScene = UnityEngine.SceneManagement.Scene;
using Core;
using Core.Signal;


namespace Core.Scene
{
   public class SceneController : ModelController, ISceneController
   {

      //private IScene m_Scene;
      //private ISignal m_SignalSceneActivate;



      public SceneController() { }
      public SceneController(params object[] args)
          => Configure(args);

      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;


      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected SceneControllerConfig m_Config;



      public IScene SceneActive { get; private set; }

      public event Action<bool> Configured;
      public event Action<bool> Initialized;

      public event Action<IScene, bool> SceneLoaded;
      public event Action<IScene, bool> SceneActivated;


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
            try { m_Config = (SceneControllerConfig)args[config]; }
            catch { Debug.LogWarning($"{this.GetName()} config was not found. Configuration failed!"); return; }
         }


         m_Config = (SceneControllerConfig)args[config];


         m_isConfigured = true;
         Configured?.Invoke(m_isConfigured);
         if (m_isDebug) Debug.Log($"{this.GetName()} configured.");
      }

      public override void Init()
      {

         m_isInitialized = true;
         Initialized?.Invoke(m_isInitialized);
         if (m_isDebug) Debug.Log($"{this.GetName()} initialized.");

      }

      public override void Dispose()
      {
         m_isInitialized = false;
         Initialized?.Invoke(m_isInitialized);
         if (m_isDebug) Debug.Log($"{this.GetName()} disposed.");
      }


      // LOAD //
      public virtual IResult SceneLoad(IScene scene)
      {
         var result = SceneLoadAsync(scene);
         SceneLoaded?.Invoke(scene, result.Status);
         return result;
      }

      public virtual void SceneUnload(IScene scene)
      {
         var loaded = false;
         SceneLoaded?.Invoke(scene, loaded);
         if (m_isDebug) Debug.Log($"{this.GetName()} unloaded.");
      }

      // ACTIVATE //
      public virtual void SceneActivate(IScene scene)
      {
         var activated = true;
         SceneActivated?.Invoke(scene, activated);
         if (m_isDebug) Debug.Log($"{this.GetName()} activated.");
      }

      public virtual void SceneDeactivate(IScene scene)
      {
         var activated = false;
         SceneActivated?.Invoke(scene, activated);
         if (m_isDebug) Debug.Log($"{this.GetName()} activated.");
      }


      // SUBSCRIBE //
      public virtual void Subscribe()
      {
         //SignalProvider.SignalCalled += OnSignalCalled;
      }

      public virtual void Unsubscribe()
      {
         //SignalProvider.SignalCalled -= OnSignalCalled;
      }


      protected virtual IResult SceneLoadAsync(IScene scene)
      {
         if (scene == null)
            return new Result(this, false, $"{scene.GetName()} not found!", m_isDebug, LogFormat.Warning);

         if (scene.isLoaded)
            return new Result(this, true, $"{scene.GetName()} is already loaded!", m_isDebug, LogFormat.Warning);

         /*await*/
         return scene.Load();
      }


      /*
      public async Task<ITaskResult> SceneActivate(IScene scene, bool animate)
      {
          if (scene == null)
              return new TaskResult(false, Send($"{scene.GetName()} not found!", LogFormat.Warning));

          if (scene.IsLoaded == false)
              return new TaskResult(false, Send($"{scene.GetName()} is not loaded.", LogFormat.Warning));

          if (scene.IsActivated == true)
              return new TaskResult(true, Send($"{scene.GetName()} is already activated.", LogFormat.Warning));

          if (SceneActive != null && SceneActive != scene)
          {
              var sceneActiveDeactivateTaskResult = await SceneActive.Deactivate();
              if (sceneActiveDeactivateTaskResult.Status == false)
                  return new TaskResult(false, Send(sceneActiveDeactivateTaskResult.Message));
          }


          if (SceneActive == scene)
              return new TaskResult(true, Send($"{scene.GetName()} was activated."));

          var sceneTargetActivateTaskResult = await scene.Activate(animate);
          if (sceneTargetActivateTaskResult.Status == false)
              return new TaskResult(false, Send(sceneTargetActivateTaskResult.Message));

          var sceneCoreDeactivateTaskResult = await m_SceneCore.Deactivate();
          if (sceneCoreDeactivateTaskResult.Status == false)
              return new TaskResult(false, Send(sceneCoreDeactivateTaskResult.Message));

          SceneActive = scene;

          return new TaskResult(true, Send($"{scene.GetName()} was activated."));



      }


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
      IResult SceneLoad(IScene scene);
      //IResult SceneActivate(IScene scene, bool animate);
   }

   public struct SceneControllerConfig : IConfig
   {

   }
}