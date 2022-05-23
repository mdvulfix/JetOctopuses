using UnityEngine;
using UnityEngine.UI;

public class ScreenGame : AScreen
{
    
    [SerializeField] private Slider m_Power;
    [SerializeField] private Slider m_Heath;
    [SerializeField] private Text m_Money; 
    
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

