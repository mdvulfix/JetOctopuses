using System;
using System.Threading.Tasks;
using APP.Scene;

namespace APP
{
    public abstract class StateModel<TState> where TState: class, IState
    {
        private bool m_Debug = true;

        private TState m_State;
        
        private IState m_StateActive;
        private IScene m_SceneActive;

        public Action<SceneIndex> SceneNeed;
        public Action<IState> StateNeed;

        public virtual Task Enter()
        {
            Send("Enter state.");
            return null;
        }

        public virtual Task Fail()
        {
            Send("Fail state.", LogFormat.Worning);
            return null;
        }

        public virtual Task Run()
        {
            Send("Exit state.");
            return null;
        }

        public virtual Task Exit()
        {
            Send("Exit state.");
            return null;
        }

        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

        
        public void OnSceneActivate(IScene scene)
        { 

        }

        public void OnStateActivate(IState scene)
        { 

        }

    }

    public class StateLoad : StateModel<StateLoad>, IState 
    { 
        public override Task Enter()
        {
            Send("System enter loading...");
            return null;
        }
        
        public override Task Run()
        {
            Send("System start loading...");

            //await Builder.Execute(new CoreBuildScheme());

            SceneNeed?.Invoke(SceneIndex<SceneCore>.Index);
            
            return null;
        }

        public override Task Exit()
        {
            Send("System complete loading...");
            return null;
        }
    }
    
    public class StateLogin : StateModel<StateLogin>, IState { }
    public class StateMenu : StateModel<StateMenu>, IState { }
    public class StateLevel : StateModel<StateLevel>, IState { }
    public class StateResult : StateModel<StateResult>, IState { }
    public class StateUnload : StateModel<StateUnload>, IState { }

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




}