using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory
{
    private IState[] m_States;

    public StateFactory()
    {
        m_States = new IState[] 
        {
            new StateLoading(),
            new StateMenu(),
            new StateGame(),
            new StatePause(),
            new StateLose(),
            new StateWin()
        };
    }

    public T Get<T>() where T: class, IState 
    { 
        foreach (var state in m_States)
        {
            if(state.GetType() == typeof(T))
                return (T)state;
        }
        
        Debug.Log("State was not found!");
        return null;
    }

}
