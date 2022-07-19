using System;

namespace APP.Signal
{
    
    public static class SignalProvider
    {
        public static event Action<ISignal> SignalCalled;

        public static void Call(ISignal signal)
        {
            SignalCalled?.Invoke(signal);
        }
    
    }
    
    
    
    
    public abstract class SignalModel<TSignal>: IConfigurable, ICacheable, ISubscriber, IMessager
    where TSignal: ISignal
    {
        private bool m_Debug = true;
        
        private SignalConfig m_Config;
        private ICacheHandler m_CacheHandler;
        
        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}

        public ISignal Signal { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<IMessage> Message;
        
        public event Action RecordRequired;
        public event Action DeleteRequired;

        
        // CONFIGURE //
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
                        m_Config = (SignalConfig) obj;
                        Signal = m_Config.Signal;
                        
                        Send($"{obj.GetName()} was setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            
      
            m_CacheHandler = new CacheHandler<ISignal>();
            
            
            
            IsConfigured = true;
            Configured?.Invoke();

            Send("Configuration completed!");   
        }
        
        public virtual void Init()
        {
            if (IsConfigured == false)
            {
                Send($"{this.GetName()} is not configured. Initialization was aborted!", LogFormat.Worning);
                return;
            }
                
            if (IsInitialized == true)
            {
                Send($"{this.GetName()} is already initialized. Current initialization was aborted!", LogFormat.Worning);
                return;
            }

            Subscribe();

            m_CacheHandler.Configure(new CacheHandlerConfig(Signal));
            m_CacheHandler.Init();

            IsInitialized = true;
            Initialized?.Invoke();
            Send("Initialization completed!");
        }

        public virtual void Dispose()
        {

            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            Send("Dispose completed!");
        }


        public virtual void Subscribe() { }

        public virtual void Unsubscribe() { }

        public virtual void Execute() { }

        public virtual void Call()
        {
            SignalProvider.Call(Signal);
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

    }

    public class SignalConfig: IConfig
    {
        public SignalConfig(ISignal signal)
        {
            Signal = signal;
        }

        public ISignal Signal {get; private set; }

    }

    public class SignalSceneActivate : SignalModel<SignalSceneActivate>, ISignal
    {
        
        public SignalSceneActivate(IScene scene) => Configure(scene);
        public SignalSceneActivate(IConfig config) => Configure(config);

        public IScene Scene {get; private set; }

        public event Action<IScene> SceneRequied;

        public void Configure(IScene scene)
        {
            Scene = scene;

            var config =  new SignalConfig(this);            
            base.Configure(config);
        }

        public override void Execute()
        {
            
            SceneRequied?.Invoke(Scene);
        }
    }


    public class SignalStateSet : SignalModel<SignalSceneActivate>, ISignal
    {
        private IState m_State;
               
        public SignalStateSet(IState state) => Configure(state);
        public SignalStateSet(IConfig config) => Configure(config);

        public IState State {get; private set; }

        public event Action<IState> StateRequied;

        public void Configure(IState state)
        {
            State = state;

            var config =  new SignalConfig(this);            
            base.Configure(config);
        }

        public override void Call()
        {
            StateRequied?.Invoke(State);
        }
    }

}
