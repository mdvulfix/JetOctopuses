using System;
using System.Collections;
using System.Collections.Generic;
using SERVICE.Handler;

namespace APP
{
    public class Register<T>: Register
    {
        public Register() { } 

        public bool Contains(T instance)
        {
            if(Contains(instance))
                return true;

            return false;
        }
        
        public void Set(T instance)
        {
            Set<T>(instance);
            Send($"{typeof(T)} added to cache.");
        }

        public void Remove(T instance)
        {
            Remove<T>(instance);
            Send($"{typeof(T)} removed from cache.");
        }
    }

    public class Register
    {
        public Register() {}

        public Register(Type objType, object instance) =>
            Set(objType, instance);
        
        private bool m_Debug = true;
        
        private readonly Dictionary<Type, object> m_Cache = new Dictionary<Type, object>(50);
        
        public bool Contains<T>()
        {            
            if(m_Cache.ContainsKey(typeof(T)))
                return true;

            var name = typeof(T).Name;
            Send($"{name} not found in register!");
            return false;
        }
         
        public bool Contains(object instance)
        {
            if(m_Cache.ContainsValue(instance))
                return true;

            var name = instance.GetType().Name;
            Send($"{name} not found in register!");
            return false;
        }
        
        public bool Get<T>(out T instance)
        where T: class
        {           
            if (m_Cache.TryGetValue(typeof(T), out var obj))
            {
                instance = (T)obj;
                return true;
            }

            instance = null;
            return false;
        }

        // SET //
        public void Set<T>(T instance) => 
            Set(typeof(T), instance);

        public void Set(Type objType, object instance)
        {
            m_Cache.Add(objType, instance);
            var name = objType.Name;
            Send($"{name} was registered!");
        }
            
        // REMOVE //
        public void Remove<T>(T instance) =>
            Remove(typeof(T), instance);


        public void Remove(Type objType, object instance)
        {   
            if(Contains(instance))
            {
                m_Cache.Remove(objType);
                var name = objType.Name;
                Send($"{name} removed from register!");
            }
        }

        protected string Send(string text, bool worning = false) =>
            LogHandler.Send(this, m_Debug, text, worning);
    }
}