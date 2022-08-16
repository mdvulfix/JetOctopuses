using System;
using UnityEngine;

namespace APP.Game.Map
{

    public class MapBoundaryController : MonoBehaviour
    {
        [SerializeField] private float m_Width = 100;
        [SerializeField] private float m_Height = 100;

        [SerializeField] private float m_OffsetTop = -15;
        [SerializeField] private float m_OffsetBottom = -1;
        [SerializeField] private float m_OffsetLeft = -1;
        [SerializeField] private float m_OffsetRight = -1;

        [SerializeField] private float m_SpawnPositionLeft;
        [SerializeField] private float m_SpawnPositionRight;
        [SerializeField] private float m_SpawnPositionOffset = 1;

        
        [SerializeField] private MapBoundary m_BoundaryTop;
        [SerializeField] private MapBoundary m_BoundaryBottom;
        [SerializeField] private MapBoundary m_BoundaryLeft;
        [SerializeField] private MapBoundary m_BoundaryRight;

        private IBoundary[] m_Boundaries;

        public event Action<IEntity> LeftBoundaryReached;
        public event Action<IEntity> RightBoundaryReached;
        public event Action<IEntity> TopBoundaryReached;
        public event Action<IEntity> BottomBoundaryReached;

        
        
        public virtual void Configure(params object[] args)
        {
            m_SpawnPositionLeft = -m_Width/2 + m_SpawnPositionOffset;
            m_SpawnPositionRight = m_Width/2 - m_SpawnPositionOffset;
            
            
            
            m_Boundaries = new MapBoundary[4]
            {
                m_BoundaryTop,
                m_BoundaryBottom,
                m_BoundaryLeft,
                m_BoundaryRight
            };

        }



        public virtual void Init()
        {
            SetBoundaries(m_Width, m_Height);
            
            foreach (var boundary in m_Boundaries)
                boundary.EntityEntered += OnBoundaryEnter;
        }

        public virtual void Dispose()
        {
            foreach (var boundary in m_Boundaries)
                boundary.EntityEntered -= OnBoundaryEnter;
        }
        
        
        public void SetBoundaries() =>
            SetBoundaries(100, 100);

        public void SetBoundaries(float width, float hight)
        {
            m_Width = width;
            m_Height = hight;

            var topPosition = new Vector3(-m_Width/2, m_Height/2 + m_OffsetTop);
            m_BoundaryTop.SetPosition(m_Width, 1, topPosition);
            m_BoundaryTop.SetIndex(BoundaryIndex.Top);


            var bottomPosition = new Vector3(-m_Width/2, -m_Height/2 + m_OffsetBottom);
            m_BoundaryBottom.SetPosition(m_Width, 1, bottomPosition);
            m_BoundaryBottom.SetIndex(BoundaryIndex.Bottom);

            
            var leftPosition = new Vector3(-m_Width/2 + m_OffsetLeft, -m_Height/2);
            m_BoundaryLeft.SetPosition(1, m_Height, leftPosition);
            m_BoundaryLeft.SetIndex(BoundaryIndex.Left);

            
            var rightPosition = new Vector3(m_Width/2, -m_Height/2);
            m_BoundaryRight.SetPosition(1, m_Height, rightPosition);
            m_BoundaryRight.SetIndex(BoundaryIndex.Right);



            Debug.Log("Boundaries was set!");

        }


        private void OnBoundaryEnter(IBoundary mapBoundary, IEntity entity)
        {
            //Debug.Log($"{entity.GetName()} entered {mapBoundary.Index.GetName()}");

            switch (mapBoundary.Index)
            {
                default : break;

                case BoundaryIndex.Left:
                    entity.SetPosition(new Vector3(m_SpawnPositionRight, entity.Position.y, entity.Position.z));
                    LeftBoundaryReached?.Invoke(entity);
                    break;

                case BoundaryIndex.Right:
                    entity.SetPosition(new Vector3(m_SpawnPositionLeft, entity.Position.y, entity.Position.z));
                    RightBoundaryReached?.Invoke(entity);
                    break;
            }
        }





        // UNITY //       
        private void Awake() =>
            Configure();

        private void OnEnable() =>
            Init();

        private void OnDisable() =>
            Dispose();

        private void Update()
        {
            //m_MapUI.SetSize(m_Width, m_Hight);
        }
    }
}