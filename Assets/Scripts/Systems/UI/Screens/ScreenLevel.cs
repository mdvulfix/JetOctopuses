using UnityEngine;
using UnityEngine.UI;

public class ScreenLevel : ScreenModel<ScreenLevel>, IScreen
{
    
    [SerializeField] private Slider m_Power;
    [SerializeField] private Slider m_Heath;
    [SerializeField] private Text m_Money; 
    
    public override void Init()
    {

    }


}

