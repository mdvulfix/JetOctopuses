using System;
using APP.Screen;

namespace APP.Scene
{
    [Serializable]
    public abstract class SceneModel<TScene> : UComponent
    where TScene : IScene
    {
        
        
        private SceneConfig m_Conig;
        private TScene m_Instance;

        private IScreenController m_ScreenController;
        
        protected SceneState StateActive { get; private set; }
        protected Action<SceneState> StateChanged { get; private set; }

        public static SceneIndex Index => SceneIndex<TScene>.Index;

        

        public event Action<IActionInfo> SceneLoaded;
        public event Action<IActionInfo> SceneUnloaded;
        public event Action<IActionInfo> SceneActivated;
        public event Action<IActionInfo> ScreenActivated;

        private Register<TScene> m_Register;

        public override void Configure (IConfig config)
        {
            if(ConfigValidate())
                return;
        
            base.Configure(config);
            m_Conig = (SceneConfig) config;
            
            
            
            m_ScreenController = new ScreenControllerDefault();

        }

        protected override void Init ()
        {
            if(InitValidate())
                return;
            
            base.Init ();
            m_ScreenController.Init();
        }

        protected override void Dispose ()
        {
            m_ScreenController.Dispose();
            base.Dispose ();
        }

        
        public void Activate<TScreen>()
            where TScreen: UComponent, IScreen
        {
            //var animate = true;
            //m_ScreenController.Activate<TScreen>(animate);
            Send("Activating screen...");
        }
        
        protected void SetScreen<TScreen>(ref TScreen screenField, IScreen[] screens)
        {
            foreach (var screen in screens)
            {
                if(screen.GetType() == typeof(TScreen))
                {
                    screenField = (TScreen)screen;
                    return;
                }
                    
                Send($"{typeof(TScreen).Name} was not found! Check scene config.", true);
            } 
            
            
        }

        private void SetState(SceneState state)
        {
            StateActive = state;
            StateChanged?.Invoke(state);
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

    
        protected enum SceneState
        {
            None,

            //Load
            LoadIn,
            LoadFail,
            LoadRun,
            LoadOut,
        
        
            //Unoad
            UnloadIn,
            UnloadFail,
            UnloadRun,
            UnloadOut,
        }
    }

    public class SceneConfig : Config
    {
        public IScreen[] Screens { get; private set; }

        public SceneConfig (InstanceInfo info, params IScreen[] screens): base(info)
        {
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