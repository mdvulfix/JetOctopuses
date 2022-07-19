using System;
using System.Collections.Generic;

namespace APP
{
    public abstract class CacheModel: IConfigurable, ISubscriber, IMessager
    {
        private bool m_Debug = false;

        private CacheConfig m_Config;
        private ICacheHandler m_Handler;

        protected static readonly Dictionary<Type, ICacheable> m_Cache = new Dictionary<Type, ICacheable>(50);

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;
        public event Action<IMessage> Message;

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
                        m_Config = (CacheConfig) obj;
                        
                        m_Handler = m_Config.Handler;
                        Send($"{obj.GetName()} setup.");
                    }
                }
            }
            else
            {
                Send("Params are empty. Config setup aborted!", LogFormat.Worning);
            }
            
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

        // SUBSCRUBE //
        public void Subscribe()
        {
            m_Handler.RecordRequired += Record;
            m_Handler.DeleteRequired += Delete;
        }

        public void Unsubscribe()
        {
            m_Handler.RecordRequired -= Record;
            m_Handler.DeleteRequired -= Delete;
        }

        // CONTAINS //
        public bool Contains<TCacheable>()
        where TCacheable : ICacheable
        {
            if (m_Cache.ContainsKey(typeof(TCacheable)))
                return true;

            Send($"{typeof(TCacheable).Name} not found in cache!");
            return false;
        }

        public bool Contains(ICacheable instance)
        {
            if (m_Cache.ContainsKey(instance.GetType()))
                return true;

            Send($"{instance.GetType().Name} not found in cache!");
            return false;
        }

        // GET //
        public TCacheable Get<TCacheable>()
        where TCacheable : ICacheable
        {
            var instance = default(TCacheable);

            if (m_Cache.TryGetValue(typeof(TCacheable), out var obj))
                instance = (TCacheable) obj;
            else
                Send($"{typeof(TCacheable).Name} was not found in cache!", LogFormat.Worning);

            return instance;
        }


        // RECORD //
        private void Record<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable =>
            Record(instance);

        private void Record(ICacheable instance)
        {
            m_Cache.Add(instance.GetType(), instance);
            Send($"{instance.GetType().Name} was set to cache!");
        }

        // DELETE //
        private void Delete<TCacheable>(TCacheable instance)
        where TCacheable : ICacheable =>
            Delete(instance);

        private void Delete(ICacheable instance)
        {
            if (Contains(instance))
            {
                m_Cache.Remove(instance.GetType());
                Send($"{instance.GetType().Name} removed from cache!");
            }
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

    public class Cache<TCacheable> : CacheModel, ICache<TCacheable>
    where TCacheable : ICacheable
    {
        public Cache() { }
        public Cache(IConfig config) => Configure(config);

        public void Configure(ICacheHandler handler)
        {              
            var config =  new CacheConfig(handler);            
            base.Configure(config);
        }
        
        // CONTAINS //
        public bool Contains() =>
            Contains<TCacheable>();

        // GET //
        public TCacheable Get() =>
            Get<TCacheable>();

    }

    public struct CacheConfig : IConfig
    {
        public CacheConfig(ICacheHandler handler)
        {
            Handler = handler;
        }

        public ICacheHandler Handler { get; private set; }
    }

    
    public interface ICache<TCacheable> : IConfigurable, ISubscriber, IMessager
    where TCacheable : ICacheable
    {
        bool Contains();
        TCacheable Get();


    }

}