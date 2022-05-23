using System;
using UnityEngine;

public class StateLoading : IState
{

    public event Action<IStateEventArgs> StateExecuted;

    public void Enter()
    {
        

        Debug.Log("System was loaded.");
    }

    public void Exit()
    {
        
    }

}
