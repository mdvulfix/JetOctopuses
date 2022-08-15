using System;
using UnityEngine;

namespace APP.Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ControlZone : MonoBehaviour, IControlZone
    {

        private float m_Radius;

        public float Radius { get => m_Radius; set => m_Radius = value; }

        public event Action<IEntity> InControlZone;
        public event Action<IEntity> OutControlZone;


        private CircleCollider2D m_Collider;


        private void Awake() 
        {
            m_Collider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (collider.TryGetComponent<IEntity>(out var entity))
                InControlZone?.Invoke(entity);
        }
        
        private void OnTriggerExit2D(Collider2D collider) 
        {
            if (collider.TryGetComponent<IEntity>(out var entity))
                OutControlZone?.Invoke(entity);
        }

    }

}

namespace APP
{
    public interface IControlZone
    {
        event Action<IEntity> InControlZone;
        event Action<IEntity> OutControlZone;
    }

}