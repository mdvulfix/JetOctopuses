using System;
using UnityEngine;

namespace APP
{
    
    public class SceneObject : MonoBehaviour, ISceneObject
    {      
        
        
        
        
        protected void SetName(string name)
        {
            
        }
        
        protected void SetParent(ISceneObject sObj)
        {

        }
    }

}

namespace APP
{
    public interface ISceneObject: IComponent
    {

    }

    public interface IComponent
    {
        GameObject gameObject { get; }

    }

}