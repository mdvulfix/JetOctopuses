using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APP.Scene;
using APP.Signal;

namespace APP
{
    public abstract class StateModel<TState>: IConfigurable, ICacheable, IMessager, ISubscriber
    where TState: class, IState
    {
        private bool m_Debug = true;

        private StateConfig m_Config;
        private ICacheHandler m_CacheHandler;

        
        private IState m_StateActive;
        private IScene m_SceneActive;

        
        private ISignal[] m_Signals;
        

        public bool IsInitialized { get; private set; }
        public bool IsConfigured { get; private set; }

        public IState State {get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<IMessage> Message;
 
        public event Action<IScene> SceneRequied;
        public event Action<IState> StateRequied;

        public event Action RecordRequired;
        public event Action DeleteRequired;
        

        public virtual void Configure(params object[] param)
        {
            if (IsConfigured == true)
            {
                Send($"{this.GetName()} was already configured. The current setup has been aborted!", LogFormat.Worning);
                return;
            }
                
            if (param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {
                    if (obj is IConfig)
                    {
                        m_Config = (StateConfig) obj;
                        State = m_Config.State;
                        
                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            
            m_CacheHandler = new CacheHandler<IState>();
            
            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");
        }
        
        public virtual void Init()
        {
            if (IsConfigured == false)
                Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);

            Subscribe();

            m_CacheHandler.Configure(new CacheHandlerConfig(State));
            m_CacheHandler.Init();


            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {
            

            
            m_CacheHandler.Dispose();
            
            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }


        public virtual void Subscribe() { }
        public virtual void Unsubscribe() { }



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

        // MESSAGE //
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message)
        {
            Message?.Invoke(message);
            return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
        }
        
        // CALLBACK //
        public void OnMessage(IMessage message) =>
            Send($"{message.Sender}: {message.Text}", message.LogFormat);

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
        private SignalSceneActivate m_SceneMenuActivate;
        private SignalStateSet m_StateMenuSet;
        
        public StateLoad() => Configure();
        public StateLoad(IConfig config) => Configure(config);

        public void Configure()
        {

            var signals = new List<ISignal>();
            signals.Add(m_SceneMenuActivate = new SignalSceneActivate(СacheProvider<SceneMenu>.Get()));
            signals.Add(m_StateMenuSet = new SignalStateSet(СacheProvider<StateMenu>.Get()));
            
            var config =  new StateConfig(this, signals.ToArray());            
            base.Configure(config);

        }
        
        
        
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

            m_SceneMenuActivate.Call();
            //return null;
        }

        public async override Task Exit()
        {
            Send("System complete loading...");
            await Task.Delay(1);
            //return null;
        }
    }

    public class StateConfig: IConfig
    {
        public string Label { get; private set; }
        public IState State {get; private set; }
        public ISignal[] Signals {get; private set; }
        
        public StateConfig(IState state, ISignal[] signals, string label = "State: ...")
        {
            Label = label;
            State = state;
            Signals = signals;
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

namespace APP
{
    public interface IState: IConfigurable, ICacheable
    {
        
        event Action<IScene> SceneRequied;
        event Action<IState> StateRequied;
        
        Task Enter();
        Task Fail();
        Task Run();
        Task Exit();
    }

}