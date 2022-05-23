using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    private StateFactory m_Factory;

    private StateController m_StateController;
    private SceneController m_SceneController;
    private Audio m_Audio;
    private Vfx m_Vfx;
    private EnemySpawner m_Spawner;

    private Score m_Score;
    
    private IState m_StateActive;

    private void Awake() 
    {
        m_Factory = new StateFactory();
        
        m_SceneController = new SceneController();
        m_StateController = new StateController();
        
        m_Audio = new Audio();
        m_Audio.Init();

        m_Vfx = new Vfx();
        m_Vfx.Init();

        m_Spawner = new EnemySpawner();
        m_Spawner.Init();

        m_Score = new Score();
        m_Score.Init(); 
    }
    
    private void OnEnable() 
    {
        m_StateController.LoadScene += OnSceneLoad;
    }

    private void OnDisable() 
    {
        m_StateController.LoadScene -= OnSceneLoad;
    }

    private void Start() 
    {
        SetState<StateMenu>();

    }


    private void Enter()
    {
        //m_StateActive.Enter();
    }

    private void Menu()
    {
       //m_StateActive.Menu();
    }

    private void Play()
    {
        //m_StateActive.Play();
    }

    private void Pause()
    {
        //m_StateActive.Pause();
    }

    private void Exit()
    {
        //m_StateActive.Exit();
    }



    private void Update() 
    {

    }


    private void SetState<T>() where T: class, IState
    { 
        m_StateController.Set<T>();
    }


    private void OnSceneLoad(SceneIndex sceneIndex)
    {
        m_SceneController.SceneLoad(sceneIndex);
    }


}
