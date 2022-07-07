using System;
using UnityEngine;

using SERVICE.Handler;
using APP.Screen;

namespace APP.Scene
{
    public abstract class SceneModel<TScene> : IConfigurable, IInitializable
    where TScene: IScene
    {
        [SerializeField]
        private bool m_Debug = true;
        
        private SceneConfig m_Config;
        
        private ISceneObject m_SceneObject;
        
        private ICacheHandler m_CacheHandler;

        private IScreenController m_ScreenController;
        private IScreen[] m_Screens;
        

        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}

        public string Name {get; private set;}
        public IScene Scene {get; private set;}
        public SceneObject SceneObject {get; private set;}

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<IActionInfo> ScreenActivated;

        // CONFIGURE //
        public virtual void Configure() =>
            Configure(config: null);

        public virtual void Configure(IConfig config) =>
            Configure(config: config, param: null);

        public virtual void Configure (IConfig config, params object[] param)
        {
            if(config != null)
            {
                m_Config = (SceneConfig) config;

                Name = m_Config.Name;
                Scene = m_Config.Scene;
            }          
               
            
            
            
            if(param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            
            m_CacheHandler = new CacheHandlerDefault<TScene>();
            m_CacheHandler.Configure(new CacheHandlerConfig(Scene));

        
            m_ScreenController = new ScreenControllerDefault();
            
            OnConfigured();
        }
        
        // INIT //
        public virtual void Init ()
        {
            if(IsConfigured == false)
            {
                Send("Instance is not configured! Initialization aborted!", LogFormat.Worning);
                return;
            }
                
            m_SceneObject = SetComponent<SceneObject>(Name);
            
            m_CacheHandler.Init();
            m_ScreenController.Init();

            OnInitialized();
        }

        public virtual void Dispose ()
        {          
            m_ScreenController.Dispose();
            m_CacheHandler.Dispose();

            OnDisposed();
        }

        
        public void Activate<TScreen>()
            where TScreen: SceneObject, IScreen
        {
            //var animate = true;
            //m_ScreenController.Activate<TScreen>(animate);
            Send("Activating screen...");
        }
        

        protected TComponent SetComponent<TComponent>() 
        where TComponent: Component, ISceneObject
        {
            return SceneHandler.SetComponent<TComponent>(m_SceneObject.gameObject);
        }

        protected TComponent SetComponent<TComponent>(string name, ISceneObject parent = null) 
        where TComponent: Component, ISceneObject
        {
            var obj = SceneHandler.SetGameObject(name, parent.gameObject);
            return SceneHandler.SetComponent<TComponent>(obj);
        }

        
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

        
        // CALLBACK //
        private void OnConfigured()
        {
            Send($"Configuration successfully completed!");
            IsConfigured = true;
            Configured?.Invoke();
        }
        
        private void OnInitialized()
        {
            Send($"Initialization successfully completed!");
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDisposed()
        {
            Send($"Dispose process successfully completed!");
            IsInitialized = false;
            Disposed?.Invoke();
        }


        // UNITY //
        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();



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
        IScreen[] Screens { get; }
    }

    public struct SceneConfig : IConfig, ISceneConfig
    {
        public string Name { get; }
        public IScene Scene { get; private set; }
        public IScreen[] Screens { get; private set; }
        

        public SceneConfig(string name, IScene scene, IScreen[] screens)
        {
            Name = name;
            Scene = scene;
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