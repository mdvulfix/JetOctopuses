using System;
using UnityEngine;


namespace Core
{
    public abstract class ModelComponent : MonoBehaviour
    {

        [Header("Stats")]
        [SerializeField] private bool m_isInitialized;

        public GameObject ObjSelf => gameObject;

        public event Action<IResult> Initialized;

        // CONFIGURE //
        public abstract void Init(params object[] args);
        public abstract void Dispose();


        public void SetParent(GameObject parent)
           => transform.SetParent(parent.transform);

        public GameObject GetParent()
           => transform.parent.gameObject;


        protected virtual void OnInitialize(IResult result, bool debug = false)
        {
            if (debug)
                Debug.Log($"{result.Context}: {result.Log}");

            m_isInitialized = result.Status;
            Initialized?.Invoke(result);

        }



    }

    public interface IComponent
    {
        GameObject ObjSelf { get; }

        void SetParent(GameObject parent);
        GameObject GetParent();


    }
}

