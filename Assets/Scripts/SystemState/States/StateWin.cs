using System;
using UnityEngine;

public class StateWin : IState
{
    
    public event Action<IStateEventArgs> StateExecuted;
    
    public void Enter()
    {
        Debug.Log("You win!");
    
    }

    public void Exit()
    {

    }
}
