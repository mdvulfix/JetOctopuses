using System;
using System.Collections;
using UnityEngine;

using SERVICE.Handler;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public abstract class SceneModel : SceneObject, IConfigurable
    {
        private static Cache<IScene> m_Cache;
        
        [SerializeField]
        private bool m_Debug = true;
        
        private ISceneConfig m_Config;
        private IScene m_Scene;
        
        private IScreenController m_ScreenController;
        private IScreen[] m_Screens;
        

        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}

      
        public event Action<ICacheable> CacheWrite;
        public event Action<ICacheable> CacheClear;

        public event Action<IActionInfo> ScreenActivated;


        // CONFIGURE //
        public virtual void Configure (IConfig config)
        {
            m_Config = (ISceneConfig) config;
            
            Index = m_Config.Index;
            m_Scene = m_Config.Instance;

            
            
        
            m_ScreenController = new ScreenControllerDefault();
            
            IsConfigured = true;
        }

        // SUBSCRUBE //
        public void Subscrube()
        {
            
            // Cache //
            CacheWrite += m_Cache.OnCacheWrite;
            CacheClear += m_Cache.OnCacheClear;
        }

        public void Unsubscrube()
        {
           
            // Cache //
            CacheWrite -= m_Cache.OnCacheWrite;
            CacheClear -= m_Cache.OnCacheClear;
        }
        
        
        // INIT //
        protected override void Init ()
        {
            if(IsConfigured == false)
            {
                Send("Instance is not configured! Initialization aborted!", LogFormat.Worning);
                return;
            }
                
            Subscrube();
            
            m_ScreenController.Init();

            Send($"Initialization successfully completed!");
            IsInitialized = true;
        }

        protected override void Dispose ()
        {
            Unsubscrube();
            
            
            m_ScreenController.Dispose();

            Send($"Dispose process successfully completed!");
            IsInitialized = false;
        }

        
    
        
        public void Activate<TScreen>()
            where TScreen: SceneObject, IScreen
        {
            //var animate = true;
            //m_ScreenController.Activate<TScreen>(animate);
            Send("Activating screen...");
        }
        
        protected TScreen Set<TScreen>(string name) where TScreen: SceneObject, IScreen
        {
            var obj = SceneHandler.SetGameObject(name, this.gameObject);
            return SceneHandler.SetComponent<TScreen>(obj);

        }

        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

        /*
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


    }

    public interface ISceneConfig
    {
        IScene Instance { get; }
        SceneIndex Index { get; }
        IScreen[] Screens { get; }
    }

    public struct SceneConfig<TScene> : IConfig, ISceneConfig 
    where TScene : IScene
    {
        public IScene Instance { get; private set; }
        public SceneIndex Index { get; private set; }

        public IScreen[] Screens { get; private set; }

        public SceneConfig(TScene instance, IScreen[] screens)
        {
            Instance = instance;
            Index = SceneIndex<TScene>.Index;

            Screens = screens;
        }
    }

    public struct SceneActionInfo : IActionInfo
    {
        public IScene Scene { get; private set; }

        public SceneActionInfo (IScene scene)
        {
            Scene = scene;
        }

    }

}