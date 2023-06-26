using System;
using System.Collections.Generic;
using Core;
using App.Scene;

namespace App
{


    public static class SceneIndex<TScene>
    where TScene : IScene
    {

        /*
        
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
                if (GetIndex(out var index))
                    return index;
                else
                    throw new SceneIndexException();
            }
        }

        public static SceneIndex SetIndex(SceneIndex index)
        {
            if (m_SceneIndexes.ContainsKey(typeof(TScene)))
                return index;

            m_SceneIndexes.Add(typeof(TScene), index);
            return index;
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


            Send($"{typeof(TScene).Name} was not found. Please update scene index handler!", LogFormat.Warning);
            return false;
        }

        public static IEnumerable<Type> GetSceneTypeArray() =>
            m_SceneIndexes.Keys;

        public static Message Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(m_Debug, "SceneIndex", text, worning);

        */
    }



}