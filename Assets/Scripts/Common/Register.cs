using System;
using System.Collections.Generic;


public class Register<T> : Register
{
    public T Instance {get; private set;}
    
    public Register(T instance) => 
        Instance = instance;

    public void Add()
    {
        Add (Instance);
        Send ($"{typeof(T)} added to cache.");
    }
    
    public void Remove()
    {
        Remove (Instance);
        Send ($"{typeof(T)} removed from cache.");
    }


    private string Send (string text, bool worning = false) =>
         Messager.Send (this, true, text, worning);


}

public class Register
{
    private readonly static Dictionary<Type, object> m_Cache = new Dictionary<Type, object> (50);

    public static bool Get (Type type, out object instance)
    {
        if (m_Cache.TryGetValue (type, out instance))
            return true;

        return false;
    }

    protected static void Add(object instance) =>
        m_Cache.Add(instance.GetType(), instance);

    protected static void Remove (object instance) =>
        m_Cache.Remove (instance.GetType ());

}
