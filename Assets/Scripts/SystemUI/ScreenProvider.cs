
public class ScreenProvider
{
    private Holder<IScreen> m_Holder;

    public ScreenProvider()
    {
        m_Holder = new Holder<IScreen>();
    }

    public bool Get<T>(out T screen) 
        where T: class, IScreen
    {
        if(m_Holder.Get(out IScreen instance))
        {
            screen = (T)instance;
            return true;
        }


        screen = null;
        return false;
    }
}