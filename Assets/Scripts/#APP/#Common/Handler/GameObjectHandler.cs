using UnityEngine;
using APP;

namespace SERVICE.Handler
{
    public static class GameObjectHandler
    {
        private static bool m_Debug = true;

        public static GameObject CreateGameObject(string name = "Unnamed", GameObject parent = null)
        {
            var obj = new GameObject(name);
            obj.SetActive(false);

            if (parent != null)
                obj.transform.SetParent(parent.transform);

            return obj;
        }

        public static void DestroyGameObject(GameObject gameObject) =>
            MonoBehaviour.Destroy(gameObject);

        public static TComponent SetComponent<TComponent>(GameObject gameObject) where TComponent : Component => 
            gameObject.AddComponent<TComponent>();

        private static Message Send(string text, LogFormat worning = LogFormat.None) =>
            Messager.Send(m_Debug, "GameObjectHandler", text, worning);

    }
}