using System.Threading.Tasks;
using APP.Player;
using APP.Scene;

namespace APP
{

    public abstract class Session<TSession> : UComponent
    where TSession : UComponent, ISession
    {
        private SessionConfig m_Config;

        
        private ISceneController m_SceneController;
        private IStateController m_StateController;


        public IState StateActive { get; private set; }
    
    
        public override void Configure(IConfig config)
        {
            
            m_Config = (SessionConfig)config;
            
            m_SceneController = new SceneControllerDefault();
            var sceneControllerInstance = new Instance(m_SceneController);
            var sceneControllerConfig = new SceneControllerConfig(sceneControllerInstance, m_Config.Scenes);
            m_SceneController.Configure(sceneControllerConfig);

            m_StateController = new StateControllerDefault();
            var stateControllerInstance = new Instance(m_StateController);
            var stateControllerConfig = new StateControllerConfig(stateControllerInstance, m_Config.States);
            m_StateController.Configure(stateControllerConfig);

            base.Configure(config);
        }
    
    
        // INIT //
        public override void Init()
        {
            base.Init();
            m_StateController.Init();
            m_SceneController.Init();
        }
    
    
        public override void Dispose()
        {
            m_SceneController.Dispose();
            m_StateController.Dispose();
            base.Dispose();
        }


        // SUBSCRUBE //
        protected override void Subscrube()
        {
            base.Subscrube();
            m_StateController.StateChanged += OnStateChanged;
        }

        protected override void Unsubscrube()
        {
            m_StateController.StateChanged -= OnStateChanged;
            base.Unsubscrube();
        }

       
        //protected virtual void StateUpdate(IState state) { }
       
        
        
        protected void StateExecute<TState>() where TState: class, IState
        {
            m_StateController.Execute<TState>();
        }


        private void OnStateChanged(IState state)
        {
            Send($"State changed. {state} state activated...");
            StateActive = state;
            //StateUpdate(StateActive);
        }
        
    }


    public class SessionDefault : Session<SessionDefault>, ISession
    {
        private SceneCore m_SceneCore;
        private SceneNet m_SceneNet;
        private SceneLogin m_SceneLogin;
        private SceneMenu m_SceneMenu;
        private SceneLevel m_SceneLevel;

    
        // CONFIGURE //
        public override void Configure(IConfig config)
        {
            SceneIndex<SceneCore>.SetIndex(SceneIndex.Core);
            SceneIndex<SceneNet>.SetIndex(SceneIndex.Net);
            SceneIndex<SceneLogin>.SetIndex(SceneIndex.Login);
            SceneIndex<SceneMenu>.SetIndex(SceneIndex.Menu);
            SceneIndex<SceneLevel>.SetIndex(SceneIndex.Level);

            base.Configure(config);
        }

        // INIT //
        public override void Init()
        {

            var instance = new Instance(this);
            
            var scenes = new IScene[5]
            {
                m_SceneCore,
                m_SceneNet,
                m_SceneLogin,
                m_SceneMenu,
                m_SceneLevel
            };
            
            var states = new IState[6]
            {
                new StateLoad(),
                new StateLogin(),
                new StateMenu(),
                new StateLevel(),
                new StateResult(),
                new StateUnload(),
            };


            var config = new SessionConfig(instance, states, scenes);
            
            
            
            Configure(config);
            base.Init();

            Send("System enter loading...");
            StateExecute<StateLoad>();
        }

        public override void Dispose()
        {
            Send("System exit...");
            StateExecute<StateUnload>();

            base.Dispose();
        }

        /*
        // STATE MANAGE //
        protected override async void StateUpdate(IState state)
        {
            switch (state)
            {
                case State.None:

                    break;

                case State.LoadIn:

                    Send("System start loading...");
                    SetState(State.LoadRun);

                    await BuildHandler.Build(new CoreBuildScheme());

                    await Task.Delay(5);

                    Send("System complete loading...");
                    SetState(State.LoadOut);

                    Send("System enter net...");
                    SetState(State.NetIn);
                    break;

                case State.NetIn:

                    Send("Build scene...");
                    await BuildHandler.Build(new NetBuildScheme());

                    //Send("System start net connection...");
                    //SetState(State.NetRun);

                    //Send("System complete net connection...");
                    //SetState(State.NetOut);

                    break;

                case State.LoginIn:

                    Send("Build scene...");
                    await BuildHandler.Build(new LoginBuildScheme());

                    Send("System start logining...");
                    //SetState(State.LoginRun);

                    //Send("System complete logining...");
                    //SetState(State.LoginOut);

                    Send("System enter menu...");
                    SetState(State.MenuIn);
                    break;

                case State.MenuIn:

                    Send("Build scene...");
                    await BuildHandler.Build(new MenuBuildScheme());

                    Send("System start menu...");
                    //SetState(State.MenuRun);

                    Send("System enter level...");
                    SetState(State.LevelIn);
                    break;

                case State.LevelIn:

                    Send("Build scene...");
                    await BuildHandler.Build(new LevelBuildScheme());

                    Send("System start level...");
                    SetState(State.LevelRun);

                    break;

                case State.LevelRun:
                    Send("In level...");

                    break;

                case State.LevelPause:
                    Send("Level pause...");

                    break;

                case State.LevelWin:
                    Send("Level win...");
                    break;

                case State.LevelLose:
                    Send("Level lose...");
                    break;

                case State.ResultIn:
                    Send("Result loading...");
                    break;

                case State.ResultRun:
                    Send("In result...");
                    break;

                default:
                    Send($"{state}: State is not implemented!", true);
                    break;
            }

        }

        

        // SCENE MANAGE //
        private async Task Activate<TScene>() where TScene : UComponent, IScene =>
            await m_SceneController.Activate<TScene>();

        // CALLBACK //
        private void OnPlayerAction(PlayerAction action)
        {
            switch (action)
            {

                case PlayerAction.MenuPlay:
                    SetState(State.LevelIn);
                    break;

                case PlayerAction.MenuExit:
                    SetState(State.MenuExit);
                    break;

                case PlayerAction.LevelPlay:
                    SetState(State.LevelRun);
                    break;

                case PlayerAction.LevelPause:
                    SetState(State.LevelPause);
                    break;

                case PlayerAction.LevelExit:
                    SetState(State.LevelExit);
                    break;

                default:
                    Send("State is not implemented!", true);
                    break;
            }

        }

        private void OnResult(Result result)
        {
            switch (result)
            {
                case Result.Win:
                    SetState(State.LevelWin);
                    break;

                case Result.Lose:
                    SetState(State.LevelLose);
                    break;

                default:
                    Send("State is not implemented!", true);
                    break;
            }

        }

        */

    }

    

    public enum Result
    {
        None,
        Win,
        Lose
    }

    public interface IState
    {
        
        Task Enter();
        Task Fail();
        Task Run();
        Task Exit();
    }

    public class SessionConfig : Config
    {
        public IState[] States {get; private set;}
        public IScene[] Scenes {get; private set;}
        
        
        public SessionConfig(
            Instance info,
            IState[] states,
            IScene[] scenes) : base(info)
        {
            States = states;
            Scenes = scenes;
        }
    }

}