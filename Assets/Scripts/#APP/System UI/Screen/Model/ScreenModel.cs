using System;
using UnityEngine;

namespace APP.Screen
{

    [Serializable]
    public abstract class ScreenModel<TScreen> : IConfigurable, IInitializable
    where TScreen : IScreen
    {
        
        [SerializeField] private bool m_Debug = true;
        
        private ScreenConfig m_Config;
        private IScreenController m_ScreenController;

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public bool IsConfigured {get; private set; }
        public bool IsInitialized {get; private set; }

        public string Name {get; private set;}
        public IScreen Scene {get; private set;}
        public SceneObject SceneObject {get; private set;}

        
        // CONFIGURE //
        public virtual void Configure (IConfig config = null, params object[] param)
        {
          

            if(config != null)
            {
                m_Config = (ScreenConfig) config;

            }          
               
            
            
            
            if(param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                    Send("Param is not used", LogFormat.Worning);
                }
            }          
                
            
            //m_CacheHandler = new CacheHandlerDefault<TScene>();
            //m_CacheHandler.Configure(new CacheHandlerConfig(Scene));

        
            m_ScreenController = new ScreenControllerDefault();
            
            OnConfigured();
        }


        public virtual void Init() 
        { 

            
            OnInitialized();
        }
        
        public virtual void Dispose()
        { 

            
            OnDisposed();
        }




        
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



        public void Activate(bool Activate = true) =>
            SceneObject.gameObject.SetActive(Activate);

        public void Animate(bool Activate = true)
        {

        }


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
        
        
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(m_Debug, this, text, worning);

        
        

    }

    public struct ScreenConfig : IConfig
    {
        public IScreen Screen { get;  private set; }
        public IButton[] Buttons { get; private set; }

        public ScreenConfig(IScreen screen, IButton[] buttons)
        {
            Screen = screen;
            Buttons = buttons;
        }
    }


    [Serializable]
    public class ScreenSplash : ScreenModel<ScreenSplash>, IScreen
    {

        public override void Init()
        {
            var buttons = new IButton[]
            {
            };

            var screenConfig = new ScreenConfig(this, buttons);
            
            base.Configure(screenConfig);
            base.Init();
        }

    }



}