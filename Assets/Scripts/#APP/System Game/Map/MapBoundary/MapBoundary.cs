using System;
using UnityEngine;

namespace APP.Game.Map
{
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class MapBoundary : MonoBehaviour, IBoundary
    {
        [SerializeField] private BoundaryIndex m_Index;

        [SerializeField] private float m_Width = 100;
        [SerializeField] private float m_Hight = 100;
        
        private Vector3 m_Position;

        public BoundaryIndex Index => m_Index;

        public event Action<IBoundary, IEntity> EntityEntered;

        private Collider2D m_Collider;
        private Transform m_Transform;

        public virtual void Configure(params object[] args)
        {
            m_Collider = GetComponent<BoxCollider2D>();
            m_Transform = GetComponent<Transform>();

        }

        public virtual void Init()
        {

        }

        public virtual void Dispose()
        {

        }
        

        public void SetPosition(float width, float hight, Vector3 position)
        {
            m_Position = position;
            m_Width = width;
            m_Hight = hight;
            
        }
        
        public void SetIndex(BoundaryIndex index)
        {
            m_Index = index;
        }
        
        
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<IEntity>(out var entity))
                EntityEntered?.Invoke(this, entity);

        }

        // UNITY //       
        private void Awake() =>
            Configure();

        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();

        private void Start()
        {
            m_Transform.position = m_Position;
            m_Transform.localScale = new Vector3(m_Width, m_Hight);
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