using System;
using System.Threading.Tasks;
using APP.Scene;
using APP.Signal;

namespace APP
{
    public abstract class StateModel<TState>
    where TState: class, IState
    {
        private bool m_Debug = true;

        private TState m_State;
        
        private IState m_StateActive;
        private IScene m_SceneActive;

        
        private ISignal[] m_Signals;

        public bool IsInitialized => throw new NotImplementedException();

        public event Action<IScene> SceneRequied;
        public event Action<IState> StateRequied;
        public event Action Initialized;
        public event Action Disposed;


        public virtual void Init()
        {

        }

        public virtual void Dispose()
        {

        }

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

        protected void SignalSend(ISignal signal)
        {
            signal.Call();
        }


    }

    public class StateLoad : StateModel<StateLoad>, IState 
    { 
        public async override Task Enter()
        {
            Send("System enter loading...");
            await Task.Delay(1);
            //return null;
        }
        
        public async override Task Run()
        {
            Send("System start loading...");
            await Task.Delay(1);

            //var signal = new SignalSceneActivate<SceneCore>();
            //SignalSend(signal);
            
            //await Builder.Execute(new CoreBuildScheme());

            //SceneNeed?.Invoke(SceneIndex<SceneCore>.Index);
            
            //return null;
        }

        public async override Task Exit()
        {
            Send("System complete loading...");
            await Task.Delay(1);
            //return null;
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