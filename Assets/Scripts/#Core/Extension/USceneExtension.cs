using UnityEngine;
using UScene = UnityEngine.SceneManagement.Scene;
using Core;

public static class USceneExtension
{
    public static bool TryGetComponentOnRootGameObject<T>(this UScene uScene, out T instance)
        where T : Component, IScene
    {
        instance = null;
        var objs = uScene.GetRootGameObjects();

        foreach (var obj in objs)
        {
            if (obj.TryGetComponent<T>(out instance))
                return true;
        }

        Debug.LogWarning($"Component {typeof(T)} was not found on uScene with index {uScene.buildIndex}!");
        return false;
    }



}
