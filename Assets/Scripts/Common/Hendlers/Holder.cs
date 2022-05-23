using System;
using System.Collections.Generic;

public class Holder<T>
{
    public event Action<T> InstanceAdded;
    public event Action<T> InstanceRemoved;
    
    
    private static Dictionary<Guid, T> m_Storage;  

    public void Add(T instance)
    {
        m_Storage.Add(typeof(T).GUID, instance);
        InstanceAdded?.Invoke(instance);
    }

    public void Remove(T instance)
    {
        m_Storage.Remove(typeof(T).GUID);
        InstanceRemoved?.Invoke(instance);
    }
    
    public bool Get(out T instance)
    {
 
        if(m_Storage.TryGetValue(typeof(T).GUID, out instance))
            return true;

        return false;
    }

}