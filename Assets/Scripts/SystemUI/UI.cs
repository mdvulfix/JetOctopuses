using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private ScreenLoading m_Loading;
    [SerializeField] private ScreenMainMenu m_MainMenu;
    [SerializeField] private ScreenOptions m_Options;
    [SerializeField] private ScreenGame m_Game;
    [SerializeField] private ScreenPause m_Pause;
    [SerializeField] private ScreenResults m_Results;
    [SerializeField] private ScreenCredits m_Credits;

    private void Awake()
    {
        
    }
    
    private void Start()
    {
        
    }


}
