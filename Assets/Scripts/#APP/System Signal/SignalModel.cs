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
    
    
    
    
    public abstract class SignalModel<TSignal>: IConfigurable, IInitializable, ICacheable, ISubscriber, IMessager
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
        
        public event Action<ICacheable> RecordToCahceRequired;
        public event Action<ICacheable> DeleteFromCahceRequired;

        public virtual IMessage Configure(IConfig config = null, params object[] param)
        {
            if (IsConfigured == true)
                return Send("The instance was already configured. The current setup has been aborted!", LogFormat.Worning);


            if(config != null)
            {
                m_Config = (SignalConfig) config;
                Signal = m_Config.Signal;
            }          
               
            if(param != null && param.Length > 0)
            {
                foreach (var obj in param)
                {   
                    if(obj is object)
                        Send("Param is not used", LogFormat.Worning);
                }
            }          
                            
            m_CacheHandler = new CacheHandler<ISignal>();
            Send(m_CacheHandler.Configure(new CacheHandlerConfig(Signal)), SendFormat.Sender);
            
            
            IsConfigured = true;
            Configured?.Invoke();
            
            return Send("Configuration completed!");
        }
        
        public virtual IMessage Init()
        {
            if (IsConfigured == false)
                return Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);

            if (IsInitialized == true)
                return Send("The instance is already initialized. Current initialization was aborted!", LogFormat.Worning);

            Subscribe();


            IsInitialized = true;
            Initialized?.Invoke();
            return Send("Initialization completed!");
        }

        public virtual IMessage Dispose()
        {

            Unsubscribe();
            
            IsInitialized = false;
            Disposed?.Invoke();
            return Send("Dispose completed!");
        }


        public virtual void Subscribe() { }

        public virtual void Unsubscribe() { }

        public virtual void Execute() { }

        public virtual void Call()
        {
            SignalProvider.Call(Signal);
        }


        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message, SendFormat sendFrom = SendFormat.Self)
        {
            Message?.Invoke(message);
            
            switch (sendFrom)
            {               
                case SendFormat.Sender:
                    return Messager.Send(m_Debug, this, $"message from: {message.Text}" , message.LogFormat);

                default:
                    return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
            }
        }
        
        // CALLBACK //
        private void OnMessage(IMessage message) =>
            Send(message);

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
