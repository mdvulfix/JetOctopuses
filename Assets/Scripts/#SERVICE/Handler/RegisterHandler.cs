using APP;

namespace SERVICE.Handler
{
    public static class RegisterHandler
    {
        private static Register m_Register = new Register();

        public static bool Get<T>(out T instance) 
        where T: UComponent
        {           
            if(m_Register.Get<T>(out instance))
                return true;

            return false;
        }

    }
}