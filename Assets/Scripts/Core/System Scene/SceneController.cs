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

        public IEnumerable<IScene> m_Scenes;

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
            m_Scenes = m_Config.Scenes;


            m_isConfigured = true;
            Configured?.Invoke(m_isConfigured);
            if (m_isDebug) Debug.Log($"{this.GetName()} configured.");
        }

        public override void Init()
        {
            foreach (var scene in m_Scenes)
            {
                scene.Init();


                //scene.LoadRequired += OnSceneLoadRequired;
                //scene.Loaded += OnSceneLoaded;
                //scene.Activated += OnSceneLoaded;
            }






            m_isInitialized = true;
            Initialized?.Invoke(m_isInitialized);
            if (m_isDebug) Debug.Log($"{this.GetName()} initialized.");

        }

        public override void Dispose()
        {

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
            Initialized?.Invoke(m_isInitialized);
            if (m_isDebug) Debug.Log($"{this.GetName()} disposed.");
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

        event Action<IScene, bool> SceneLoaded;
        event Action<IScene, bool> SceneActivated;

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