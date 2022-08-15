using System;
using UnityEngine;

namespace APP.Game.Map
{
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class MapBoundary : MonoBehaviour, IBoundary
    {
        [SerializeField] private BoundaryIndex m_Index;

        private int m_Hight;
        private int m_Width;
        private Vector3 m_Position;

        public BoundaryIndex Index => m_Index;

        public event Action<IBoundary, IEntity> EntityEntered;

        private Collider2D m_Collider;

        private void Awake()
        {
            m_Collider = GetComponent<BoxCollider2D>();

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<IEntity>(out var entity))
                EntityEntered?.Invoke(this, entity);

        }
    }


}

namespace APP.Game
{

    public interface IBoundary
    {
        BoundaryIndex Index {get; }
        event Action<IBoundary, IEntity> EntityEntered;
    }
    
    public enum BoundaryIndex
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

}