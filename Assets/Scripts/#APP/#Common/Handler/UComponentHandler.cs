using UnityEngine;
using APP;

namespace SERVICE.Handler
{
    public static class UComponentHandler
    {
        public static TComponent SetComponent<TComponent>(GameObject gameObject)
        where TComponent : UComponent
        {
            return gameObject.AddComponent<TComponent>();
        }

        public static GameObject CreateGameObject(string name = "Unnamed", GameObject parent = null)
        {
            var obj = new GameObject(name);
        
            if (parent != null)
                obj.transform.SetParent(parent.transform);

            return obj;
        }

    }
}