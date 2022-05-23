using System;
using System.Collections.Generic;
using UnityEngine;

public class Cachable<T>: Cachable
{
    
    protected void Set()
    {
        m_Storage.Add(typeof(T), this);
        Send($"{typeof(T)} added to cache.");
    }

    protected void Del()
    {
        m_Storage.Remove(typeof(T));
        Send($"{typeof(T)} removed from cache.");
    }

    private string Send(string text, bool worning = false)
    { 
        return Messager.Send(this, true, text, worning);
    }

}


public class Cachable: MonoBehaviour
{

    protected static Dictionary<Type, Cachable> m_Storage = new Dictionary<Type, Cachable>(50);

    public static bool Get(Type type, out Cachable instance)
    {

        if (m_Storage.TryGetValue(type, out instance))
        {
            return true;
        }
        
        return false;
    }
}



public class CachHandler<T>
    where T: Cachable
{

    public bool Get(out T instance)
    {
        var type = typeof(T);
        instance = null;

        if (Cachable.Get(type, out var cachable))
        {
            instance = (T)cachable;
            return true;
        }

        Send($"{type} was not found!", true);
        return false;
    }

    private string Send(string text, bool worning = false)
    { 
        return Messager.Send(this, true, text, worning);
    }

}

public interface ICachable
{

}