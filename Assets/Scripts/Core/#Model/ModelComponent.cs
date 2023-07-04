using System;
using UnityEngine;


namespace Core
{
    public abstract class ModelComponent : MonoBehaviour
    {
        [SerializeField] public GameObject ObjSelf => gameObject;


        // CONFIGURE //
        public abstract void Init(params object[] args);
        public abstract void Dispose();


        public void SetParent(GameObject parent)
           => transform.SetParent(parent.transform);

        public GameObject GetParent()
           => transform.parent.gameObject;
    }

    public interface IComponent
    {
        GameObject ObjSelf { get; }

        void SetParent(GameObject parent);
        GameObject GetParent();


    }
}

