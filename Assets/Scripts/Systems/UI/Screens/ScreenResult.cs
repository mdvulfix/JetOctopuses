using UnityEngine;
using UnityEngine.UI;

public class ScreenResult : ScreenModel<ScreenResult>,IScreen
{
    
    [SerializeField] private Button m_Menu;
    [SerializeField] private Button m_Credits;
    

    public override void Init()
    {

    }
}
