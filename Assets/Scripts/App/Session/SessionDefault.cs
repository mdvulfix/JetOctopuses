using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;
using Core.Scene;

using App.Scene;
using App.State;

namespace App
{
    public enum SceneIndex
    {
        Core,
        Login,
        Menu,
        Level
    }

    [Serializable]
    public class SessionDefault : SessionModel, ISession
    {

        [SerializeField] private StateLogin m_StateLogin;
        [SerializeField] private StateMenu m_StateMenu;
        private IState m_StateActive;
        private List<IState> m_States;


        [SerializeField] private SceneLogin m_SceneLogin;
        [SerializeField] private SceneMenu m_SceneMenu;
        [SerializeField] private SceneLevel m_SceneLevel;
        private IScene m_SceneActive;
        private List<IScene> m_Scenes;


        // CONFIGURE //
        public override void Configure(params object[] param)
        {
            Debug.Log("Session configuring...");

            var config = new SessionConfig(this);
            base.Configure(config);

        }

        public override void Init()
        {
            Debug.Log("Session initializing...");

            m_States = new List<IState>(10);
            m_States.Add(m_StateLogin = StateLogin.Get());
            m_States.Add(m_StateMenu = StateMenu.Get());

            m_Scenes = new List<IScene>(10);
            m_Scenes.Add(m_SceneLogin = SceneLogin.Get());
            m_Scenes.Add(m_SceneMenu = SceneMenu.Get());


            foreach (var scene in m_Scenes)
            {
                scene.Configure();
                scene.Init();

                //scene.LoadRequired += OnSceneLoadRequired;
                //scene.Loaded += OnSceneLoaded;
                //scene.Activated += OnSceneLoaded;
            }




            SetState(m_StateLogin);

            SceneActivate(m_SceneLogin);

            base.Init();
        }


        public override void Dispose()
        {
            Debug.Log("Session disposing...");
            foreach (var scene in m_Scenes)
            {
                //scene.Activated -= OnSceneLoaded;
                //scene.Loaded -= OnSceneLoaded;
                //scene.LoadRequired -= OnSceneLoadRequired;
                scene.Dispose();
            }


            m_StateActive = null;
            m_Scenes.Clear();

            m_SceneActive = null;
            m_States.Clear();

            base.Dispose();
        }


        private void SetState(IState state)
        {
            m_StateActive?.Exit();

            m_StateActive = state;
            m_StateActive.Enter();
            OnStateChanged();

            m_StateActive.Run();

        }

        private void SceneLoad(IScene scene) { }
        //=> RunAsync(scene?.Load());

        private void SceneActivate(IScene scene) { }
        //=> RunAsync(scene?.Activate());



        private void OnStateChanged()
        {

            /*
            try
            {
                switch (m_State)
                {
                    case StateIndex.MenuLoading:
                        SceneLoad(SceneIndex.Menu, () => SetState(StateIndex.MenuActivating));
                        break;

                    case StateIndex.MenuActivating:
                        SceneActivate(SceneIndex.Menu, () => SetState(StateIndex.MenuRun));
                        break;

                    case StateIndex.MenuRun:
                        Debug.Log("Current state " + m_State);
                        break;

                    case StateIndex.LevelLoading:

                        SceneLoad(SceneIndex.Level, () => SetState(StateIndex.LevelActivating));
                        break;

                    case StateIndex.LevelActivating:
                        SceneActivate(SceneIndex.Level, () => SetState(StateIndex.LevelRun));
                        break;

                    case StateIndex.LevelRun:
                        Debug.Log("Current state " + m_State);
                        break;

                    default:
                        throw new Exception("State is not implemented!");

                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
            finally
            {
                Debug.Log("State was changed! Current state: " + m_State.ToString());
            }

            */


        }

        private void OnStateRequired(IState state)
            => SetState(state);

        private void OnSceneLoadRequired(IScene scene)
        {
            SceneLoad(scene);
        }



        private void OnSceneLoaded(IScene scene, bool result)
        {
            Debug.Log($"{scene.GetName()} load status changed to {result}!");
        }

        private void OnSceneActivated(IScene scene, bool result)
        {
            if (result)
                m_SceneActive = scene;

            Debug.Log($"{scene.GetName()} activation status changed to {result}!");
        }

    }

    /*
    public enum StateIndex
    { 
        None,
        MenuLoading,
        MenuActivating,
        MenuRun,
        LevelLoading,
        LevelActivating,
        LevelRun
    }

    public enum SceneIndex
    { 
        None,
        Menu,
        Level
    }


        */






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
    Draw,
    Win,
    Lose
}
