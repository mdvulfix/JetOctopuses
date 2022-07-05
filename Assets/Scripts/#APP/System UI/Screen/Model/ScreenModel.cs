using System;
using UnityEngine;

namespace APP.Screen
{

    [Serializable]
    public abstract class ScreenModel<TScreen> : SceneObject, IConfigurable
    where TScreen : IScreen
    {
        
        [SerializeField] private bool m_Debug = true;
        
        private ScreenConfig m_Config;
        private IScreenController m_ScreenController;

        public bool IsConfigured {get; private set; }

        public void Configure(IConfig config)
        {
            
            
            m_Config = (ScreenConfig) config;

        }

        protected override void Init() { }
        protected override void Dispose() { }


        
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

    public class ScreenConfig : Config
    {
        public IButton[] Buttons { get; private set; }

        public ScreenConfig(IScreen screen, IButton[] buttons): base(screen)
        {
            Buttons = buttons;
        }
    }


    [Serializable]
    public class ScreenSplash : ScreenModel<ScreenSplash>, IScreen
    {

        protected override void Init()
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