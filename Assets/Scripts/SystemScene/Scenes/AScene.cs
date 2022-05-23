using System;
using UnityEngine;

public abstract class AScene : MonoBehaviour, IScene
{
    private Holder<IScene> m_Holder;

    public event Action<bool> SceneActivated;

    private void Awake() 
    {
        OnAwake();
    }

    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected abstract void OnAwake();

    protected void Add(IScene instance)
    {
        m_Holder.Add(instance);
        Activate(true);
    }

    protected void Remove(IScene instance)
    {
        Activate(false);
        m_Holder.Remove(instance);
    }

    private void Activate(bool activate)
    {
        SceneActivated?.Invoke(activate);
    }


}
