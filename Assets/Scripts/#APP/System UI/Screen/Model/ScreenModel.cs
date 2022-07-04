using System;
using APP.Button;

namespace APP.Screen
{

    [Serializable]
    public abstract class ScreenModel<TScreen> : UComponent, IConfigurable
    where TScreen : IScreen
    {
        private ScreenConfig m_Config;
        private IScreenController m_ScreenController;

        public override void Configure(IConfig config)
        {
            base.Configure(config);
            m_Config = (ScreenConfig) config;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Dispose()
        {

            base.Dispose();
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

    
    
    
    }

    public class ScreenConfig : Config
    {
        public IButton[] Buttons { get; private set; }

        public ScreenConfig(Instance info, IButton[] buttons): base(info)
        {
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

            var Instance = new Instance(this);
            var screenConfig = new ScreenConfig(Instance, buttons);
            
            base.Configure(screenConfig);
            base.Init();
        }

    }



}