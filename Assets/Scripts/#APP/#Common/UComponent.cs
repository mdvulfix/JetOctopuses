using System;
using SERVICE.Handler;
using UnityEngine;

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


        // CONFIGURE //
        public virtual void Configure(IConfig config)
        {
            m_Config = (Config)config;
            m_Register = new Register();
        }

        protected virtual void Init()
        {
            Subscrube();
            OnInitialize(m_Config.InstanceInfo);
        }

        protected virtual void Dispose()
        {
            OnDispose(m_Config.InstanceInfo);
            Unsubscrube();
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
            Send($"{name} initialization process successfully completed!");
        }
            
        private void OnDispose(InstanceInfo info) 
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


    
    
    }

}