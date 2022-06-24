using UnityEngine;
using APP;

namespace SERVICE.Handler
{
    public class SceneObjectHandler
    {
        public SceneObjectHandler() =>
            Create();

        public SceneObjectHandler(string name) =>
            Create(name);

        public SceneObjectHandler(string name, GameObject parent) =>
            Create(name, parent);

        public GameObject Create(string name = "Unnamed", GameObject parent = null)
        {
            var obj = new GameObject(name);
        
            if (parent != null)
                obj.transform.SetParent(parent.transform);

            return obj;
        }

        public TComponent SetComponent<TComponent>(GameObject gameObject, IConfig config)
        where TComponent : SceneObject, IConfigurable
        {
            var tComponent = gameObject.AddComponent<TComponent>();
            tComponent.Configure(config);
            return tComponent;
        }

    }
}