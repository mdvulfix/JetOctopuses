using System;
using System.Threading.Tasks;
using UnityEngine;

using Core.Cache;

namespace Core.UI
{

    public abstract class ViewModel : ModelComponent
    {
        [SerializeField]
        private bool m_IsDebug;

        private ViewConfig m_Config;
        private IScene m_Scene;

        //private ViewProvider<TView> m_Provider;
        private ViewController m_UIController;

        private SceneState m_StateActive;
        private SceneState m_StateNext;

        private ICacheHandler m_CacheHandler;
        private static ICache m_Cache = new Cache<IView>(new CacheConfig());




        public string Label => "View";

        public event Action<SceneState> StateChanged;
        public event Action<IScene> SceneRequired;


        // CONFIGURE //
        public override void Init(params object[] args)
        {
            var config = (int)Params.Config;

            if (args.Length > 0)
                try { m_Config = (ViewConfig)args[config]; }
                catch { Debug.LogWarning($"{this}: {Label} config was not found. Configuration failed!"); return; }

            m_Scene = m_Config.Scene;


            m_CacheHandler = new CacheHandler(new CacheHandlerConfig());
            m_Cache.SetObserver(m_CacheHandler);

            //var sceneProviderConfig = new SceneProviderConfig(m_Scene);
            //m_SceneProvider = new SceneProvider<TScene>(sceneProviderConfig);
            //m_SceneProvider.Init();

            base.Init();
        }


        public override void Dispose()
        {

            m_Cache.RemoveObserver(m_CacheHandler);
            m_CacheHandler.Dispose();

            base.Dispose();
        }



        // ACTIVATE //
        public override void Activate()
        {
            Obj.SetActive(true);
            base.Activate();
        }

        public override void Deactivate()
        {
            Obj.SetActive(false);
            base.Deactivate();
        }



        public async Task Enter<TScreen>()
            where TScreen : Component, IScreen
        {

            //var screen = m_ScreenController.ScreenActive;

            //if (screen is TScene)
            //   return;

            //await m_ScreenController.Deactivate();
            // await m_ScreenController.Activate<TScreen>();
            return;
        }

    }




    public struct ViewConfig : IConfig
    {
        public IScene Scene { get; private set; }

        public ViewConfig(IScene scene)
        {
            Scene = scene;
        }


    }



}