using UnityEngine;
using UScene = UnityEngine.SceneManagement.Scene;
using Core;

public static class ObjectExtension
{
    public static string GetName(this object instance) =>
        instance.GetType().Name;

    public static string Send(this object instance, object context, LogFormat format)
    {
        if (instance is string)
        {
            switch (format)
            {
                case LogFormat.Warning:
                    Debug.LogWarning($"{context.GetName()}: {instance}");
                    break;
                case LogFormat.Error:
                    Debug.LogError($"{context.GetName()}: {instance}");
                    break;

                default:
                    Debug.Log($"{context.GetName()}: {instance}");
                    break;
            }

            return (string)instance;
        }

        return null;
    }

}
