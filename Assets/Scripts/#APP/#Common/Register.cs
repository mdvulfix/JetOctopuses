using System;
using System.Collections.Generic;
using SERVICE.Handler;

namespace APP
{
    public class Register<T> : Register
    {
        private T m_Instance;

        public Register(T instance) =>
            m_Instance = instance;

        public void Set()
        {
            m_Cache.Add(typeof(T), m_Instance);
            Send($"{typeof(T)} added to cache.");
        }

        public void Remove()
        {
            m_Cache.Remove(typeof(T));
            Send($"{typeof(T)} removed from cache.");
        }

        private string Send(string text, bool worning = false) =>
            LogHandler.Send(this, true, text, worning);

    }

    public class Register
    {
        protected readonly static Dictionary<Type, object> m_Cache = new Dictionary<Type, object>(50);

        public static bool Get<T>(out T instance)
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
    }
}