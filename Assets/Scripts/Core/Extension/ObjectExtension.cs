using UnityEngine;
using UScene = UnityEngine.SceneManagement.Scene;
using App;

public static class ObjectExtension
{
    public static string GetName(this object instance) =>
        instance.GetType().Name;

}
