using System;
using UnityEngine;

public abstract class AScreen : MonoBehaviour, IScreen
{
    private Holder<IScreen> m_Holder;

    public event Action<bool> ScreenActivated;

    private void Awake() 
    {
        OnAwake();
    }

    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected abstract void OnAwake();


    protected void Add(IScreen instance)
    {
        m_Holder.Add(instance);
        Activate(true);
    }

    protected void Remove(IScreen instance)
    {
        Activate(false);
        m_Holder.Remove(instance);
    }


    private void Activate(bool activate)
    {
        ScreenActivated?.Invoke(activate);
    }

    protected void Animate(bool animate)
    {
        
    }




}
