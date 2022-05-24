using System;
using System.Collections.Generic;
using UnityEngine;

public class Register<T> : Register
    where T: SceneObject
{

    public void Add (T instance)
    {
        Add (instance);
        Send ($"{typeof(T)} added to cache.");
    }

    public void Remove (T instance)
    {
        Remove (instance);
        Send ($"{typeof(T)} removed from cache.");
    }

    private string Send (string text, bool worning = false)
    {
        return Messager.Send (this, true, text, worning);
    }

}

public class Register
{
    private readonly static Dictionary<Type, object> m_Cache = new Dictionary<Type, object> (50);

    public static bool Get (Type type, out object instance)
    {
        if (m_Cache.TryGetValue (type, out instance))
        {
            return true;
        }

        return false;
    }

    protected static void Add (object instance)
    {
        m_Cache.Add (instance.GetType (), instance);
    }

    protected static void Remove (object instance)
    {
        m_Cache.Remove (instance.GetType ());
    }

}

public class SceneObject : MonoBehaviour
{
    private Register m_Register;

    private Animator m_Animator;

    public void Activate (bool Activate = true)
    {
        gameObject.SetActive (Activate);
    }

    public void Animate (bool Activate = true)
    {

    }

}