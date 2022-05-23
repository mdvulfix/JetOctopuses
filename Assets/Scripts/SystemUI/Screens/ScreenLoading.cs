using UnityEngine;
using UnityEngine.UI;

public class ScreenLoading : AScreen
{
    
    [SerializeField] private Slider m_Progress;
    
    
    protected override void OnAwake()
    {

    }

    protected override void OnEnable()
    {
        Add(this);
    }

    protected override void OnDisable()
    {
        Remove(this);
    }



}
