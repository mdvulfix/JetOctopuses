using System;
using UnityEngine;
using SERVICE.Handler;

namespace APP
{
    public abstract class UComponent : MonoBehaviour, IConfigurable
    {
        [SerializeField]
        private bool m_Debug = true;
        
        private Register m_Register;
        private Config m_Config;

        protected Action<Type, object> Initialized;
        protected Action<Type, object> Disposed;

        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}



        // CONFIGURE //
        public virtual void Configure(IConfig config)
        {
            if(ConfigValidate())
                return;
            
            m_Config = (Config)config;
            m_Register = new Register();

            IsConfigured = true;
        }

        protected virtual void Init()
        {
            if(InitValidate())
                return;
            
            Subscrube();
            OnInitialize(m_Config.InstanceInfo);

            IsInitialized = true;
        }

        protected virtual void Dispose()
        {
            OnDispose(m_Config.InstanceInfo);
            Unsubscrube();

            IsInitialized = false;
        }
           
        protected virtual void Run()
        {

        }


        public void Activate(bool Activate = true) =>
            gameObject.SetActive(Activate);

        public void Animate(bool Activate = true)
        {

        }
 
        protected void SetName(string name)
        {
            
        }
        
        protected void SetParent(UComponent component)
        {

        }
    
        // VALIDATION //
        protected bool ConfigValidate()
        {
            if(IsConfigured == true)
            {
                Send("Component already configured.");
                return true;
            }
            
            return false;
        }
        
        protected bool InitValidate()
        {

            if(IsConfigured == false)
            {
                Send("Component must be configured before initialization!", true); 
                return true;
            }
            
            if(IsInitialized == true)
            {
                Send("Component already initialized."); 
                return true;
            }
            
            return false;
        }

        
        protected string Send(string text, bool worning = false) =>
            LogHandler.Send(this, m_Debug, text, worning);

        
        protected virtual void Subscrube()
        {
            Initialized += m_Register.Set;
            Disposed += m_Register.Remove;
        }
        
        protected virtual void Unsubscrube()
        {
            Initialized -= m_Register.Set;
            Disposed -= m_Register.Remove;
        }

        private void OnInitialize(InstanceInfo info)
        {
            Initialized?.Invoke(info.ObjType,info.Obj);
            var name = info.ObjType.Name;
            Send($"Initialization successfully completed!");
        }
            
        private void OnDispose(InstanceInfo info) 
        {
            var name = info.ObjType.Name;
            Send($"{name} dispose process successfully completed!");
            Disposed?.Invoke(info.ObjType,info.Obj);
        } 
            
        /*
        // UNITY //
        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();

        private void Start() =>
            Run();
        */
        
        void IConfigurable.Init()
        {
            throw new NotImplementedException();
        }

        void IConfigurable.Dispose()
        {
            throw new NotImplementedException();
        }
    }

}