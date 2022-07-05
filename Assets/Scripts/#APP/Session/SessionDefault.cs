using APP.Player;
using APP.Scene;
using APP.Signal;

namespace APP
{

    public class SessionDefault : SessionModel<SessionDefault>, ISession
    {
    
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
            var states = new IState[6]
            {
                new StateLoad(),
                new StateLogin(),
                new StateMenu(),
                new StateLevel(),
                new StateResult(),
                new StateUnload(),
            };
            
            var config = new SessionConfig(this, states);
            
            
            
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

    
        public enum Result
        {
            None,
            Win,
            Lose
        }
    
    }

    



    public class SessionConfig : Config
    {
        public IState[] States {get; private set;}
        
        
        public SessionConfig(
            ISession session,
            IState[] states) : base(session)
        {
            States = states;

        }
    }

}