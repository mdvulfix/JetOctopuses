using System;
using System.Threading.Tasks;
using UnityEngine;
using SERVICE.Handler;
using SERVICE.Builder;
using APP.Scene;
using APP.Player;

namespace APP
{
    public class SessionDefault : UComponent, ISession
    {
        private State StateActive;

        
        private IBuilder m_Builder;
        private ISceneController m_SceneController;

        public bool IsConfigured { get; private set; }

        public event Action<State> StateChanged;

        // CONFIGURE //
        public override void Configure(IConfig config)
        {
            if(IsConfigured == true)
                return;
            
            base.Configure(config);
            
            m_Builder = RegisterHandler.Get<BuilderDefault>();
            m_SceneController = new SceneControllerDefault();

            IsConfigured = true;
        }
        
        protected override void Init()
        {
            var info = new InstanceInfo(this);
            var config = new SessionConfig(info, m_SceneController);
            
            Configure(config);
            base.Init();

            Subscrube();


            m_SceneController.Init();

            Send("System enter loading...");
            SetState(State.LoadIn);
        }

        protected override void Dispose()
        {
            m_SceneController.Dispose();

            Send("System exit...");
            SetState(State.UnloadIn);

            Unsubscrube();
            base.Dispose();
        }

        // SUBSCRUBE //
        protected override void Subscrube()
        {
            base.Subscrube();
            StateChanged += OnStateChanged;
        }
        
        protected override void Unsubscrube()
        {
            
            StateChanged -= OnStateChanged;
            base.Unsubscrube();
        }

        // STATE MANAGE //
        private async void HandleState(State state)
        {
            switch (state)
            {
                case State.None:

                    break;

                case State.LoadIn:

                    Send("System start loading...");
                    SetState(State.LoadRun);

                    await Task.Delay(5);
                    
                    Send("System complete loading...");
                    SetState(State.LoadOut);

                    Send("System enter net...");
                    SetState(State.NetIn);
                    break;

                
                case State.NetIn:

                    Send("System start net connection...");
                    SetState(State.NetRun);

                    
                    var scheme = new BuildSchemeNet(SceneIndex.Net);
                    await m_Builder.Build(scheme);

                    Send("System complete net connection...");
                    SetState(State.NetOut);
                    
                    Send("System enter login...");
                    //SetState(State.MenuIn);
                    break;
                
                
                
                case State.LoginIn:

                    Send("System start logining...");
                    SetState(State.LoginRun);

                    await Activate<SceneLogin>();

                    Send("System complete logining...");
                    SetState(State.LoginOut);
                    
                    Send("System enter menu...");
                    SetState(State.MenuIn);
                    break;

                case State.MenuIn:

                    Send("System start menu...");
                    SetState(State.MenuRun);

                    await Activate<SceneMenu>();
                    
                    break;


                case State.LevelIn:
                    Send("Level loading...");
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

        private void SetState(State state)
        {
            StateActive = state;
            StateChanged?.Invoke(state);
        }

       // SCENE MANAGE //
        private async Task Activate<TScene>() where TScene: UComponent, IScene =>
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

        private void OnStateChanged(State state)
        {
            Send($"State changed. {state} state activated...");
            HandleState(state);
        }

    }
    public class SessionConfig : Config
    {
        public ISceneController SceneController { get; private set; }

        public SessionConfig(InstanceInfo info, ISceneController sceneController) : base(info)
        {
            SceneController = sceneController;
        }
    }

    public enum State
    {
        None,

        //Load
        LoadIn,
        LoadFail,
        LoadRun,
        LoadOut,

        //Net
        NetIn,
        NetFail,
        NetRun,
        NetExit,
        NetOut,
        
        //Login
        LoginIn,
        LoginFail,
        LoginRun,
        LoginExit,
        LoginOut,

        //Menu
        MenuIn,
        MenuFail,
        MenuRun,
        MenuExit,
        MenuOut,

        //Level
        LevelIn,
        LevelFail,
        LevelRun,
        LevelWin,
        LevelLose,
        LevelPause,
        LevelExit,
        LevelOut,

        //Result
        ResultIn,
        ResultFail,
        ResultRun,
        ResultExit,
        ResultOut,

        //Unload
        UnloadIn,
        UnloadFail,
        UnloadRun,
        UnloadOut,

    }

    public enum Result
    {
        None,
        Win,
        Lose
    }

}