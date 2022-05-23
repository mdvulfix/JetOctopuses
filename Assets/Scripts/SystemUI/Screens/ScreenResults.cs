using UnityEngine;
using UnityEngine.UI;

public class ScreenResults : AScreen
{
    
    [SerializeField] private Button m_Menu;
    [SerializeField] private Button m_Credits;
    

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
