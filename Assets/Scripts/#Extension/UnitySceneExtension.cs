﻿using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace Core
{
    public static class UnitySceneExtension
    {
        public static bool TryGetComponentOnRootGameObject<T>(this UnityScene scene, out T instance)
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
