using UnityEngine;

namespace APP
{
    public interface ISceneObject: IComponent
    {
        SceneIndex Index { get; set; }
    }

    public interface IComponent
    {
        GameObject gameObject { get; }

    }

}