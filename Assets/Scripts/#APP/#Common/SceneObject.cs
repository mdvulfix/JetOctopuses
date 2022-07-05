using UnityEngine;

namespace APP
{
    public abstract class SceneObject : MonoBehaviour
    {
        
        
        public SceneIndex Index {get; protected set;}
 
        protected abstract void Init();
        protected abstract void Dispose();

        protected void SetName(string name)
        {
            
        }
        
        protected void SetParent(SceneObject sObj)
        {

        }
    
        // UNITY //
        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();

    }

}