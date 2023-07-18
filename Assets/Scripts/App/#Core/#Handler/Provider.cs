using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Provider
{

    /*
    public class Provider<T> : Provider, IProvider
    where T : ICacheable
    {

        //protected static Dictionary<Type, IScene> m_Scenes = new Dictionary<Type, IScene>(10);

        public bool Get(out T instance)
        {
            if (Get(typeof(T), out var cacheable))
            {
                instance = (T)cacheable;
                return true;
            }

            instance = default(T);
            return false;
        }

    }



    public class Provider
    {

        //protected static Dictionary<Type, IScene> m_Scenes = new Dictionary<Type, IScene>(10);

        public bool Get(Type type, out ICacheable instance)
        {

            instance = null;


            var sceneType = typeof(TScene);
            if (Get(sceneType, out var instance))
            {
                scene = (TScene)instance;
                return true;
            }
    

            return false;
        }



        public bool Contains(Type type)
        {
            return false;

        }


    }

    */

}