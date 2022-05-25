using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class SceneObject : MonoBehaviour
{
    private Register m_Register;

    private Animator m_Animator;

    private void Awake() 
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable ()
    {
        Init ();
    }

    private void OnDisable ()
    {
        Dispose ();
    }

    protected virtual void Init() { }
    protected virtual void Dispose() { }


    public void Activate (bool Activate = true) =>
        gameObject.SetActive (Activate);

    public void Animate (bool Activate = true)
    {

    }

}