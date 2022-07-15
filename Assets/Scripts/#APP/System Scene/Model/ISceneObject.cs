using UnityEngine;

namespace APP
{
    public interface ISceneObject: IComponent
    {

    }

    public interface IComponent
    {
        GameObject gameObject { get; }

    }

}