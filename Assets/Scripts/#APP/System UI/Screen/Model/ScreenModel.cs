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

        
        // CONFIGURE //
        public virtual void Configure() =>
            Configure(config: null);

        public virtual void Configure(IConfig config) =>
            Configure(config: config, param: null);

        public virtual void Configure (IConfig config, params object[] param)
        {
            if(CheckConfigure() == false)
                return;
            

            if(config != null)
            {
                m_Config = (ScreenConfig) config;

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
        
        
        
        
        
        
        
        
        
        
        public virtual void Configure(IConfig config)
        {
            if(IsConfigured == true)
                return;

            m_Config = (ScreenConfig) config;


            
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
            gameObject.SetActive(Activate);

        public void Animate(bool Activate = true)
        {

        }




        
        

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