using System;
using System.Collections.Generic;
using SERVICE.Handler;
using APP.Scene;

namespace APP
{
    public static class SceneIndex<TScene>
    where TScene: IScene
    {
        
        private static bool m_Debug = true;
        
        private static readonly Dictionary<Type, SceneIndex> m_SceneIndexes = 
            new Dictionary<Type, SceneIndex>(10)
            {
                { typeof(SceneCore), SceneIndex.Core }
            };
        
        public static SceneIndex Index 
        { 
            get 
            {
                if(GetIndex(out var index))
                    return index;
                else
                    throw new SceneIndexException();
            } 
        } 

        public static void SetIndex(SceneIndex index)
        {
            if (m_SceneIndexes.ContainsKey(typeof(TScene)))
                return;
            
            m_SceneIndexes.Add(typeof(TScene), index);
        }
            
        public static bool GetIndex(out SceneIndex sceneIndex)
        {
            sceneIndex = default(SceneIndex);
            
            if (m_SceneIndexes.ContainsKey(typeof(TScene)))
            {
                m_SceneIndexes.TryGetValue(typeof(TScene), out var index);
                sceneIndex = (SceneIndex)index;
                return true;
            }

            
            Send($"{typeof(TScene).Name} was not found. Please update scene index handler!", true);
            return false;
        }

        public static IEnumerable<Type> GetSceneTypeArray() =>
            m_SceneIndexes.Keys;
        
        private static string Send(string text, bool isWorning = false) =>
            LogHandler.Send("SceneIndex", m_Debug, text, isWorning);

    }

    public enum SceneIndex
    {
        Service,
        Core,
        Net,
        Login,
        Menu,
        Level
    }

}