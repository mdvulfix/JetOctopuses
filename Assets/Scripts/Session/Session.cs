using System;
using UnityEngine;

public class Session : SceneObject, IConfigurable
{

    [SerializeField] 
    private bool m_Debug;
    
    private SceneController m_SceneController;
   
    
    private event Action<State> StateChanged;

    
    private void Awake ()
    {
        Configure(new SessionConfig());
    }

    private void OnEnable ()
    {
        Set();
        Init();
    }

    private void OnDisable ()
    {
        Dispose();
        Del();
    }


    public void Configure (IConfig config)
    {
        m_SceneController = new SceneController ();
    }
    
    
    public void Init ()
    {
        StateChanged += HandleState;
        m_SceneController.Init();

        


        SetState(State.LoadIn);
    }

    public void Dispose()    
    {
        m_SceneController.Dispose();
        SetState(State.UnloadIn);
        


        StateChanged -= HandleState;
    }
    
    
    
    private void HandleState(State state)
    {
        switch (state)
        {
            case State.None:

                break; 

            case State.LoadIn:

                Send("System loading...");
                SetState(State.LoginIn);
                
                
                break;  

            case State.LoginIn:

                Send("Login loading...");
                Load(SceneIndex.Login);
                
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

    private void SetState (State state)
    {
        OnStateChanged(state);
        
    }

    private void Load(SceneIndex scene)
    {
        m_SceneController.Load(scene);

    }

    private void Unload(SceneIndex scene)
    {
        m_SceneController.Unload(scene);

    }



    private void OnPlayerAction (PlayerAction action)
    {
        switch (action)
        {

            case PlayerAction.MenuPlay:
                SetState (State.LevelIn);
                break;

            case PlayerAction.MenuExit:
                SetState (State.MenuExit);
                break;


            case PlayerAction.LevelPlay:
                SetState (State.LevelRun);
                break;

            case PlayerAction.LevelPause:
                SetState (State.LevelPause);
                break;

            case PlayerAction.LevelExit:
                SetState (State.LevelExit);
                break;

            default:
                Send ("State is not implemented!", true);
                break;
        }

    }

    private void OnResult (Result result)
    {
        switch (result)
        {
            case Result.Win:
                SetState (State.LevelWin);
                break;

            case Result.Lose:
                SetState (State.LevelLose);
                break;

            default:
                Send ("State is not implemented!", true);
                break;
        }

    }

    private void OnStateChanged (State state)
    {
        Send ($"State changed. {state} state activated...");
        StateChanged?.Invoke (state);

    }

    private string Send(string text, bool worning = false)
    { 
        return Messager.Send(this, m_Debug, text, worning);
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




public enum PlayerAction
{
    None,
    Login,
    MenuMain,
    MenuOptions,
    MenuPlay,
    MenuExit,
    LevelPlay,
    LevelPause,
    LevelExit,
    ResultExit
}

public enum Result
{
    None,
    Win,
    Lose
}

public enum SceneIndex
{
    Service,
    Core,
    Login,
    Menu,
    Level,
    Result
}


public class SessionConfig: IConfig
{
    public SessionConfig()
    {

    }
}



public interface IConfigurable
{
    void Configure(IConfig config);
}

public interface IConfig
{

}