using System;
using System.Collections.Generic;
using SERVICE.Handler;
using APP.Scene;

namespace APP
{
    public static class State<T>
    {
        
        private static bool m_Debug = true;
        
        private static readonly List<Type> m_States = 
            new List<Type>(10)
            {

            };
        
        private static StateIndex Index 
        { 
            get 
            {
                if(GetState(out var state))
                    return state;
                else
                    throw new SceneIndexException();
            } 
        } 

        private static void SetState(StateIndex index)
        {

        }
            
        private static bool GetState(out StateIndex sceneIndex)
        {
            sceneIndex = default(StateIndex);
            
            
            Send($"{typeof(T).Name} was not found. Please update scene index handler!", true);
            return false;
        }


        
        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("State", m_Debug, text, isWorning);

    
    
        private enum StateIndex
        {
            None
        }
    
    }



}