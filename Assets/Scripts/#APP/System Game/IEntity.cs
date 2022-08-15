using UnityEngine;

namespace APP
{
    public interface IEntity
    {
        Vector3 Position {get; }

        void SetPosition(Vector3 position);
    }

}