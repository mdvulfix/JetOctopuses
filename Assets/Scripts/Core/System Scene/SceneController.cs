using System;
using System.Threading.Tasks;
using UnityEngine;
using UScene = UnityEngine.SceneManagement.Scene;
using Core;
using Core.Signal;
using System.Collections.Generic;
using System.Collections;

namespace Core.Scene
{
   public class SceneController : ModelController, ISceneController
   {

      //private IScene m_Scene;
      //private ISignal m_SignalSceneActivate;





      [Header("Stats")]
      [SerializeField] private bool m_isConfigured;
      [SerializeField] private bool m_isInitialized;


      [Header("Debug")]
      [SerializeField] protected bool m_isDebug = true;

      [Header("Config")]
      [SerializeField] protected SceneControllerConfig m_Config;

      public IEnumerable<IScene> m_Scenes;

      public event Action<IResult> Configured;
      public event Action<IResult> Initialized;
      public event Action<IResult> SceneLoaded;
      public event Action<IResult> SceneActivated;



      public SceneController() { }
      public SceneController(params object[] args)
          => Configure(args);


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
            try { m_Config = (SceneControllerConfig)args[config]; }
            catch { $"{this.GetName()} config was not found. Configuration failed!".Send(this, m_isDebug, LogFormat.Warning); return; }
         }

         m_Config = (SceneControllerConfig)args[config];
         m_Scenes = m_Config.Scenes;



         m_isConfigured = true;
         log = $"{this.GetName()} configured.";
         result = new Result(this, m_isConfigured, log, m_isDebug);
         Configured?.Invoke(result);
      }

      public override void Init()
      {
         var result = default(IResult);
         var log = "...";

         foreach (var scene in m_Scenes)
         {
            scene.Init();


            //scene.LoadRequired += OnSceneLoadRequired;
            //scene.Loaded += OnSceneLoaded;
            //scene.Activated += OnSceneLoaded;
         }


         m_isInitialized = true;
         log = $"{this.GetName()} initialized.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }

      public override void Dispose()
      {
         var result = default(IResult);
         var log = "...";

         foreach (var scene in m_Scenes)
         {


            scene.Deactivate();
            scene.Unload();
            scene.Dispose();


            //scene.LoadRequired += OnSceneLoadRequired;
            //scene.Loaded += OnSceneLoaded;
            //scene.Activated += OnSceneLoaded;
         }

         m_isInitialized = false;
         log = $"{this.GetName()} disposed.";
         result = new Result(this, m_isInitialized, log, m_isDebug);
         Initialized?.Invoke(result);

      }


      // LOAD //
      public virtual void SceneLoad(IScene scene)
      {
         if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
         if (scene.isLoaded) { $"{scene.GetName()} is already loaded!".Send(this, m_isDebug, LogFormat.Warning); return; }

         scene.Load();
      }

      public virtual void SceneUnload(IScene scene)
      {
         if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
         if (!scene.isLoaded) { $"{scene.GetName()} is already unloaded!".Send(this, m_isDebug, LogFormat.Warning); return; }

         scene.Unload();
      }

      // ACTIVATE //
      public virtual void SceneActivate(IScene scene)
      {
         if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
         if (!scene.isLoaded) { $"{scene.GetName()} is not loaded!".Send(this, m_isDebug, LogFormat.Warning); return; }
         if (scene.isActivated) { $"{scene.GetName()} is already activated!".Send(this, m_isDebug, LogFormat.Warning); return; }

         scene.Activate();

      }

      public virtual void SceneDeactivate(IScene scene)
      {

         if (scene == null) { $"{scene.GetName()} not found!".Send(this, m_isDebug, LogFormat.Warning); return; }
         if (!scene.isLoaded) { $"{scene.GetName()} is not loaded!".Send(this, m_isDebug, LogFormat.Warning); return; }
         if (!scene.isActivated) { $"{scene.GetName()} is already deactivated!".Send(this, m_isDebug, LogFormat.Warning); return; }

         scene.Deactivate();
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

      void SceneLoad(IScene scene);
      void SceneActivate(IScene scene);
      //IResult SceneActivate(IScene scene, bool animate);
   }

   public struct SceneControllerConfig : IConfig
   {
      public SceneControllerConfig(IEnumerable<IScene> scenes)
      {
         Scenes = scenes;
      }

      public IEnumerable<IScene> Scenes { get; private set; }
   }
}