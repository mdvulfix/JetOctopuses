using UnityEngine;
using UnityEngine.UI;

public class ScreenPause : AScreen
{
    
    [SerializeField] private Button m_Resume;
    [SerializeField] private Button m_Options;
    [SerializeField] private Button m_Menu;
    

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
