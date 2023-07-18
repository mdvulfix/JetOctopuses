using UnityEngine;
using SceneInfo = UnityEngine.SceneManagement.Scene;

namespace Core
{
    public static class SceneInfoExtension
    {
        public static bool TryGetComponentOnRootGameObject<T>(this SceneInfo scene, out T instance)
            where T : Component, IScene
        {
            instance = null;
            var objs = scene.GetRootGameObjects();

            foreach (var obj in objs)
            {
                if (obj.TryGetComponent<T>(out instance))
                    return true;
            }

            Debug.LogWarning($"Component {typeof(T)} was not found on scene with build index {scene.buildIndex}!");
            return false;
        }



    }
}

