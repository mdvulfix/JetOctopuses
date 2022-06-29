using APP;

namespace SERVICE.Handler
{
    public static class RegisterHandler
    {
        private static Register m_Register = new Register();

        public static bool Contains<T>() 
        where T: UComponent
        {           
            if(m_Register.Get<T>(out var instance))
                return true;

            return false;
        }

        public static T Get<T>() 
        where T: UComponent
        {           
            T instance = null;
            if(m_Register.Contains<T>())
                m_Register.Get<T>(out instance);

            return instance;
        }
    }
}