using System;
using UnityEngine;

public class StateLose: IState
{
    
    public event Action<IStateEventArgs> StateExecuted;
    
    public void Enter()
    {
        Debug.Log("You lose!");
    
    }

    public void Exit()
    {

    }

}
