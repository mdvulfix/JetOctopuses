using System;
using UnityEngine;

public class StatePause : IState
{
    
    public event Action<IStateEventArgs> StateExecuted;
    
    public void Enter()
    {
        Debug.Log("Pause...");
    
    }

    public void Exit()
    {

    }
}
