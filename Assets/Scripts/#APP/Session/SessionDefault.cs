using System;
using System.Collections.Generic;
using UnityEngine;

using APP.Scene;


namespace APP
{
    [Serializable]
    public class SessionDefault : SessionModel<SessionDefault>, ISession
    {
        [Header("Scenes: ")]
        [SerializeField] private SceneCore m_SceneCore;
        [SerializeField] private SceneLogin m_SceneLogin;
        [SerializeField] private SceneMenu m_SceneMenu;
        [SerializeField] private SceneLevel m_SceneLevel;
        
        [Header("States: ")]
        [SerializeField] private StateLoad m_StateLoad;
        [SerializeField] private StateLogin m_StateLogin;
        [SerializeField] private StateMenu m_StateMenu;
        [SerializeField] private StateLevel m_StateLevel;
        [SerializeField] private StateResult m_StateResult;        
        [SerializeField] private StateUnload m_StateUnload; 
        
        
        private IScene m_SceneActive;
        
        
        // CONFIGURE //
        public IMessage Configure()
        {
            Send("Start configuration ...");
            
            var scenes =  new List<IScene>();
            //scenes.Add(m_SceneCore = new SceneCore());
            scenes.Add(m_SceneLogin = new SceneLogin());
            //scenes.Add(m_SceneMenu = new SceneMenu());
            //scenes.Add(m_SceneLevel = new SceneLevel());
            
            var states = new List<IState>();
            //states.Add(m_StateLoad = new StateLoad());
            //states.Add(m_StateLogin = new StateLogin());
            //states.Add(m_StateMenu = new StateMenu());
            //states.Add(m_StateLevel = new StateLevel());
            //states.Add(m_StateResult = new StateResult());
            //states.Add(m_StateUnload = new StateUnload());
            
            var config =  new SessionConfig(this, scenes.ToArray(), m_SceneLogin, m_SceneMenu, m_SceneLevel, states.ToArray());            
            return base.Configure(config);
        }
        
        
        // INIT //
        public override IMessage Init()
        {                        
            Send("Start initialization...");
            return base.Init();

            //Send("System enter loading...");
            //StateExecute<StateLoad>();
        }

        public override IMessage Dispose()
        {
            //Send("System exit...");
            //StateExecute<StateUnload>();

            return base.Dispose();
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
        // UNITY //
        private void Awake() =>
            Configure();

        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();
    
        public enum Result
        {
            None,
            Win,
            Lose
        }
    
    }

    public struct SessionConfig : IConfig
    {
        public string Label { get; private set;}
        public ISession Session { get; private set;}
        public IScene[] Scenes { get; private set;}
        public IScene SceneLogin { get; private set;}
        public IScene SceneMenu { get; private set;}
        public IScene SceneLevel { get; internal set; }
        public IState[] States {get; private set;}
        

        public SessionConfig(
            ISession session,
            IScene[] scenes,
            IScene sceneLogin,
            IScene sceneMenu,
            IScene sceneLevel,
            IState[] states,
            string label = "Session")
        {
            Label = label;
            Session = session;
            Scenes = scenes;
            SceneLogin = sceneLogin;
            SceneMenu = sceneMenu;
            SceneLevel = sceneLevel;
            States = states;
        }
    }
}