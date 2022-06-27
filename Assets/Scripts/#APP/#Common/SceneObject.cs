using UnityEngine;

namespace APP
{
    public class SceneObject<T> : MonoBehaviour, IConfigurable
    {

        private SceneObjectConfig<T> m_Config;
        private Register<T> m_Register;

        public virtual void Configure(IConfig config)
        {
            m_Config = (SceneObjectConfig<T>)config;
            m_Register = new Register<T>(m_Config.Instance);
        }
        
        
        
        protected virtual void Init () =>
            m_Register.Set ();
        protected virtual void Dispose () => 
            m_Register.Remove ();


        protected void Activate (bool Activate = true) =>
            gameObject.SetActive (Activate);

        protected void Animate (bool Activate = true)
        {

        }


        // UNITY

        private void OnEnable () =>
            Init ();

        private void OnDisable () =>
            Dispose ();


    }

    public class SceneObjectConfig<T> : IConfig
    {
        public T Instance { get; private set; }
        
        public string Name { get; private set; }
        public GameObject Parent { get; private set; }

        public SceneObjectConfig (T instance, string name, GameObject parent)
        {
            Instance = instance;
            Name = name;
            Parent = parent;
        }

    }

}