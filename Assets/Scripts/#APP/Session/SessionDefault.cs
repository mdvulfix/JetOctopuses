using System;
using UnityEngine;
using SERVICE.Handler;
using APP.Scene;
using APP.Screen;
using APP.Player;

namespace APP
{
    public class SessionDefault : SessionModel<SessionDefault>, ISession
    {
        private State StateActive;

        private ISceneController m_SceneController;

        protected override void Init()
        {
            //Scene controller...
            m_SceneController = new SceneControllerDefault();

            var info = new InstanceInfo(this);
            var config = new SessionConfig(info, m_SceneController);
            base.Configure(config);
            base.Init();

            StateChanged += OnStateChanged;
            m_SceneController.Init();

            Send("System enter...");
            SetState(State.LoadIn);
        }

        protected override void Dispose()
        {
            m_SceneController.Dispose();

            Send("System exit...");
            SetState(State.UnloadIn);

            StateChanged -= OnStateChanged;
            base.Dispose();
        }

        protected override void HandleState(State state)
        {
            switch (state)
            {
                case State.None:

                    break;

                case State.LoadIn:

                    Send("System loading...");
                    SetState(State.LoadRun);

                    Activate<SceneMenu, ScreenLoading>();

                    Send("System complete loading...");
                    SetState(State.LoadOut);

                    Send("System enter logining...");
                    SetState(State.LoginIn);
                    break;

                case State.LoginIn:

                    Send("System logining...");
                    SetState(State.LoginRun);

                    Activate<SceneMenu, ScreenLogin>();

                    Send("System complete logining...");
                    SetState(State.LoginOut);
                    break;

                case State.MenuIn:

                    Send("Menu loading...");
                    break;

                case State.MenuRun:
                    Send("In menu...");

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

        protected override void SetState(State state)
        {
            StateActive = state;
            //StateChanged?.Invoke(state);
        }

        private void Activate<TScene, TScreen>()
            where TScene : UComponent, IScene
            where TScreen : UComponent, IScreen => m_SceneController.Activate<TScene, TScreen>();

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

}