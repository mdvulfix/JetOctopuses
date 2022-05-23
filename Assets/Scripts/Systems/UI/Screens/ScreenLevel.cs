using UnityEngine;
using UnityEngine.UI;

public class ScreenLevel : AScreen<ScreenLevel>
{
    
    [SerializeField] private Slider m_Power;
    [SerializeField] private Slider m_Heath;
    [SerializeField] private Text m_Money; 
    
    protected override void Init()
    {

    }


}

