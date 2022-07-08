using System;
using UnityEngine;

namespace APP
{
    public class SceneObject : MonoBehaviour, ISceneObject
    {
        public SceneIndex Index {get; set;}

        protected void SetName(string name)
        {
            
        }
        
        protected void SetParent(ISceneObject sObj)
        {

        }
    
    }

}