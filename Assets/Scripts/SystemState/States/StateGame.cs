using System;
using UnityEngine;

public class StateGame : IState
{
    
    public event Action<IStateEventArgs> StateExecuted;
    
    public StateGame()
    {

    }

    public void Enter()
    {

    }
    public void Exit()
    {

    }
}
