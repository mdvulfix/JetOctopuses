using System;
using UnityEngine;

namespace APP
{
    
    public class SceneObject : MonoBehaviour, ISceneObject
    {      
        public void SetName(string name)
        {
            gameObject.name = name;
        }
        
        public void SetParent(ISceneObject sObj)
        {
            gameObject.transform.parent = sObj.gameObject.transform;
        }

        public void Destroy()
        {
            Destroy(gameObject);
            //Destroy(this);
        }
    }

}

namespace APP
{
    public interface ISceneObject : IComponent
    {
        void SetName(string name);
        void SetParent(ISceneObject sObj);
        void Destroy();
    }

    public interface IComponent
    {
        GameObject gameObject { get; }

    }

}