using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Core.Scene;
using Core.Async;
using Core.UI;

namespace Core
{

    [Serializable]
    public class SessionDefault : SessionModel, ISession
    {

        //[SerializeField] private StateLogin m_StateLogin;
        //[SerializeField] private StateMenu m_StateMenu;
        //private IState m_StateActive;
        //private List<IState> m_States;


        [SerializeField] private SceneLogin m_SceneLogin;
        [SerializeField] private SceneMenu m_SceneMenu;


        private SystemState m_StateActive;
        private SystemState m_StateNext;


        private ISceneController m_SceneController;
        private IAsyncController m_AsyncController;
        private IInputController m_InputController;
        private IViewController m_ViewController;


        public void Subscribe()
        {
            StateChanged += HandleState;

            //m_SceneController.SceneActivated += OnSceneActivated;
            //m_InputController.ActionActivated += OnPlayerAction;
            //m_UIController.ActionActivated += OnPlayerAction;
        }

        public void Unsubscribe()
        {
            //m_SceneController.SceneActivated -= OnSceneActivated;
            //m_InputController.ActionActivated -= OnPlayerAction;
            //m_UIController.ActionActivated -= OnPlayerAction;

            StateChanged -= HandleState;
        }


        // CONFIGURE //

        public override void Init(params object[] args)
        {
            Debug.Log("Session initializing...");

            Subscribe();



            //m_States = new List<IState>(10);
            //m_States.Add(m_StateLogin = StateLogin.Get());
            //m_States.Add(m_StateMenu = StateMenu.Get());



            m_AsyncController = new AsyncController();
            m_AsyncController.Init(new AsyncControllerConfig());




            var sceneFactory = new SceneFactory();

            //m_SceneController = new SceneController();
            //m_SceneController.Init(new SceneControllerConfig(sceneFactory));


            //m_InputController.Init();
            //m_ViewController.Init();

            //SetState(m_StateLogin);

            //SceneActivate(m_SceneLogin);







            //TaskHandler.Init();


            OnSystemAction(SystemAction.Load, Send("System loading..."));




            base.Init(new SessionConfig());
        }

        public override void Dispose()
        {
            "Session disposing...".Send(this, m_isDebug);

            Close();


            //m_AsyncController.Dispose();
            m_SceneController.Dispose();
            m_InputController.Dispose();
            m_UIController.Dispose();

            // m_StateActive = null;
            // m_Scenes.Clear();

            // m_SceneActive = null;
            // m_States.Clear();


            // TaskHandler.Dispose();

            Unsubscribe();





            base.Dispose();

        }


        private async Task Menu<TScreen>()
        where TScreen : Component, IScreen
        {
            if (m_StateActive == SystemState.MenuLoadSuccess)
                return;

            //await Enter<SceneMenu, TScreen>();
            SetState(SystemState.MenuLoadSuccess);
        }

        private async Task Level<TScreen>()
        where TScreen : Component, IScreen
        {
            if (m_StateActive == SystemState.LevelLoadSuccess)
                return;

            //await Enter<SceneLevel, TScreen>();

            SetState(SystemState.LevelLoadSuccess);

        }


        public override void HandleState(SystemState state)
        {
            switch (state)
            {
                case SystemState.None:

                    break;

                case SystemState.LoadIn:
                    //await Core<ScreenLoading>();
                    OnSystemAction(SystemAction.Login);
                    Debug.Log("System logining...");
                    break;

                case SystemState.LoginIn:
                    //await Core<ScreenLoading>();
                    OnSystemAction(SystemAction.MenuLoad);
                    Debug.Log("System loading main menu...");
                    break;

                case SystemState.MenuLoadIn:
                    //await Menu<ScreenLoading>();
                    OnSystemAction(SystemAction.MenuRun);
                    Debug.Log("System in main menu!");
                    break;

                case SystemState.MenuMain:
                    //await Menu<ScreenMenuMain>();
                    //OnSystemAction(SystemAction.LevelLoad, Send("System in level!"));

                    break;

                case SystemState.MenuOptions:
                    //await Menu<ScreenMenuOptions>();

                    break;


                case SystemState.LevelLoadIn:
                    //await Level<ScreenLoading>();
                    OnSystemAction(SystemAction.LevelRun);
                    Debug.Log("System in level!");
                    break;

                case SystemState.LevelRun:
                    //await Level<ScreenLevelRuntime>();

                    break;

                case SystemState.LevelPause:
                    //await Level<ScreenLevelPause>();

                    break;

                case SystemState.LevelWin:
                    //await Enter<SceneTotals>(SystemState.TotalsLoadSuccess);
                    break;

                case SystemState.LevelLose:
                    //await Enter<SceneTotals>(SystemState.TotalsLoadSuccess);
                    break;

                case SystemState.TotalsRun:
                    Debug.Log("In totals state!");
                    break;

                case SystemState.DisposeIn:
                    Dispose();
                    break;

                default:
                    Debug.LogWarning($"{state}: System state behavior is not implemented!");
                    break;
            }
        }

        public override void HandleInput()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
                m_InputController.HandleKeyInput(KeyCode.Alpha1);

            if (Input.GetKeyUp(KeyCode.Alpha2))
                m_InputController.HandleKeyInput(KeyCode.Alpha2);

            if (Input.GetKeyUp(KeyCode.O))
                m_InputController.HandleKeyInput(KeyCode.O);

            if (Input.GetKeyUp(KeyCode.E))
                m_InputController.HandleKeyInput(KeyCode.E);
        }



        #region Callbacks
        /*
        private void OnSignalCalled(ISignal signal)
        {
            OnPlayerAction(signal.PlayerAction);
            Send($"{signal} was called! Player action {signal.PlayerAction} was send!");

        }
        */


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



        private void OnStateChanged(SystemState state)
        {
            Send($"State changed. {state} state activated...");
            StateChanged?.Invoke(state);

        }

        private void OnSystemAction(SystemAction action)
        {
            switch (action)
            {
                case SystemAction.None:

                    break;

                case SystemAction.Load:
                    SetState(SystemState.LoadIn);
                    break;

                case SystemAction.Login:
                    SetState(SystemState.LoginIn);
                    break;

                case SystemAction.MenuLoad:
                    SetState(SystemState.MenuLoadIn);
                    break;

                case SystemAction.MenuRun:
                    SetState(SystemState.MenuMain);
                    break;

                case SystemAction.LevelLoad:
                    SetState(SystemState.LevelLoadIn);
                    break;

                case SystemAction.LevelRun:
                    SetState(SystemState.LevelRun);
                    break;


                default:
                    SetState(SystemState.None);
                    break;
            }

        }

        private void SetState(SystemState state)
        {
            m_StateActive = state;
            OnStateChanged(state);
        }

        private void OnSceneActivated(IScene scene, string message)
        {
            if (scene == null)
                return;


            else
                Send($"{scene.GetType()}: System state behavior is not implemented!", true);
        }

        private void OnPlayerAction(PlayerAction action)
        {
            switch (action)
            {

                case PlayerAction.MenuPlay:
                    SetState(SystemState.LevelLoadIn);
                    break;


                case PlayerAction.MenuOptions:
                    SetState(SystemState.MenuOptions);
                    break;

                case PlayerAction.MenuMain:
                    SetState(SystemState.MenuMain);
                    break;

                case PlayerAction.MenuExit:
                    SetState(SystemState.DisposeIn);
                    break;

                case PlayerAction.LevelPause:
                    SetState(SystemState.LevelPause);
                    break;

                case PlayerAction.LevelPlay:
                    SetState(SystemState.LevelRun);
                    break;

                case PlayerAction.LevelExit:
                    SetState(SystemState.MenuLoadIn);
                    break;

                default:
                    Send("State not found!", true);
                    break;
            }

        }

        private void OnPlayerResult(PlayerResult results, string message)
        {
            switch (results)
            {
                case PlayerResult.Win:
                    SetState(SystemState.LevelWin);
                    break;

                case PlayerResult.Lose:
                    SetState(SystemState.LevelLose);
                    break;

                default:
                    Send("State not found!", true);
                    break;
            }

        }


        private void OnStateRequired(IState state)
            => SetState(state);

        private void OnSceneLoadRequired(IScene scene)
        {
            //SceneLoad(scene);
        }

        #endregion

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







public enum PlayResult
{
    None,
    Draw,
    Win,
    Lose
}
