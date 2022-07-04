using System;
using UnityEngine;
using SERVICE.Handler;

namespace APP
{
    public abstract class UComponent : MonoBehaviour, IConfigurable
    {
        [SerializeField]
        private bool m_Debug = true;
        
        private Config m_Config;

        protected Action<Type, object> Initialized;
        protected Action<Type, object> Disposed;

        public bool IsConfigured {get; private set;}
        public bool IsInitialized {get; private set;}


        // CONFIGURE //
        public virtual void Configure(IConfig config)
        {            
            m_Config = (Config)config;

            IsConfigured = true;
        }

        public virtual void Init()
        {
            
            Subscrube();
            OnInit(m_Config.Instance);

            IsInitialized = true;
        }

        public virtual void Dispose()
        {
            OnDispose(m_Config.Instance);
            Unsubscrube();

            IsInitialized = false;
        }
           
        protected virtual void Load()
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
    
        
        protected string Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(this, m_Debug, text, worning);

        
        protected virtual void Subscrube() { }
        protected virtual void Unsubscrube() { }


        private void OnInit(Instance info)
        {
            Initialized?.Invoke(info.ObjType,info.Obj);
            var name = info.ObjType.Name;
            Send($"{name} initialization successfully completed!");
        }
            
        private void OnDispose(Instance info) 
        {
            var name = info.ObjType.Name;
            Send($"{name} dispose process successfully completed!");
            Disposed?.Invoke(info.ObjType,info.Obj);
        } 
            

        // UNITY //

        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();

        private void Start() =>
            Load();

    }

}