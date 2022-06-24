using UnityEngine;

namespace APP
{
    public class SceneObject : MonoBehaviour
    {
        protected virtual void Init() { }
        protected virtual void Dispose() { }


        protected void Activate(bool Activate = true) =>
            gameObject.SetActive(Activate);

        protected void Animate(bool Activate = true)
        {

        }

        // UNITY
        private void OnEnable() =>
            Init();
    
        private void OnDisable() =>
            Dispose();


    }

    public class SceneObjectConfig : IConfig
    {
        public string Name { get; private set; }
        public GameObject Parent { get; private set; }

        public SceneObjectConfig(string name, GameObject parent)
        {
            Name = name;
            Parent = parent;
        }

    }

}