using System;
using UnityEngine;

public class StateMenu: IState 
{
    private SceneIndex m_LoadSceneIndex;
    
    public event Action<IStateEventArgs> StateExecuted;
    
    public StateMenu()
    {
        m_LoadSceneIndex = SceneIndex.Menu;
    }

    public void Enter()
    {
        StateExecuted?.Invoke(new LoadSceneEventArgs(this, m_LoadSceneIndex));
        Debug.Log("Menu was started.");
    }
    public void Exit()
    {

    }

}
