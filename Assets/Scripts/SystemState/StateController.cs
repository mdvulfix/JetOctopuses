using System;
using UnityEngine;

public class StateController 
{
    
    event Action<IStateEventArgs> StateExecuted;
    

    public event Action<SceneIndex> LoadScene;

    private StateFactory m_Factory;
    private IState m_ActiveState;

    public StateController()
    {
        m_Factory = new StateFactory();
        StateEnter<StateLoading>();
    }

    public void Set<T>() where T: class, IState
    {
        StateExit();
        StateEnter<T>();

    }

    private void StateEnter<T>() where T: class, IState
    {
        m_ActiveState = m_Factory.Get<T>();
        //m_ActiveState.StateExecuted += OnStateExecuted;
        //m_ActiveState.Enter();
    }
    
    private void StateExit()
    {
        //m_ActiveState.Exit();
        //m_ActiveState.StateExecuted -= OnStateExecuted;
    }
    
    private void OnStateExecuted(IStateEventArgs args)
    {
        if (args is LoadSceneEventArgs)
            LoadScene?.Invoke(((LoadSceneEventArgs)args).SceneIndex);
    }

}


public struct LoadSceneEventArgs: IStateEventArgs
{
    public IState State { get; }
    public SceneIndex SceneIndex { get; }

    public LoadSceneEventArgs(IState state, SceneIndex sceneIndex)
    {
        State = state;
        SceneIndex = sceneIndex;
    }
}

public struct StateEventArgs: IStateEventArgs
{
    public IState State { get; }

    public StateEventArgs(IState state)
    {
        State = state;
    }
}

public interface IStateEventArgs
{
}


