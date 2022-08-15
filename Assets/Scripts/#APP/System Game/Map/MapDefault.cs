using System;
using UnityEngine;

namespace APP.Game.Map
{
    public class MapDefault : MapModel<MapDefault>, IMap
    {
        [SerializeField] private MapBoundary m_BoundaryTop;
        [SerializeField] private MapBoundary m_BoundaryBottom;
        [SerializeField] private MapBoundary m_BoundaryLeft;
        [SerializeField] private MapBoundary m_BoundaryRight;
        
        
        public MapDefault() { }
        public MapDefault(params object[] args) => Configure(args);

        public override void Configure(params object[] args)
        {
            var width = 250;
            var hight = 100;
            

            var boundaries = new MapBoundary[4]
            {
                m_BoundaryTop,
                m_BoundaryBottom,
                m_BoundaryLeft,
                m_BoundaryRight
            };

            var config = new MapConfig(hight, width, boundaries);
            base.Configure(config);
        }

    }

    public class MapModel<TMap> : MonoBehaviour, IConfigurable
    {
        
        [SerializeField] private MapUI m_MapUI;
        
        [SerializeField] private int m_Hight;
        [SerializeField] private int m_Width;

        [SerializeField] private float m_BoundarySizeTop;
        [SerializeField] private float m_BoundarySizeBottom;
        [SerializeField] private float m_BoundarySizeLeft;
        [SerializeField] private float m_BoundarySizeRight;
        [SerializeField] private float m_BoundaryHightOffset;
        [SerializeField] private float m_BoundaryWidthOffset;


        private MapConfig m_Config;
        private IBoundary[] m_Boundaries;

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public int Hight => m_Hight;
        public int Width => m_Width;

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;

        public event Action<IEntity> LeftBoundaryReached;
        public event Action<IEntity> RightBoundaryReached;
        public event Action<IEntity> TopBoundaryReached;
        public event Action<IEntity> BottomBoundaryReached;

        public virtual void Configure(params object[] args)
        {
            m_Config = (MapConfig) args[0];
            m_Hight = m_Config.Hight;
            m_Width = m_Config.Width;
            m_Boundaries = m_Config.Boundaries;

            m_BoundaryHightOffset = -2;
            m_BoundaryWidthOffset = -2;

            m_BoundarySizeTop = m_Hight / 2 + m_BoundaryHightOffset;
            m_BoundarySizeBottom = -m_Hight / 2 + m_BoundaryHightOffset;
            m_BoundarySizeLeft = - m_Width / 2 - m_BoundaryWidthOffset;
            m_BoundarySizeRight = m_Width / 2 + m_BoundaryWidthOffset;

        }

        public void Init()
        {
            foreach (var boundary in m_Boundaries)
                boundary.EntityEntered += OnBoundaryEnter;
        }

        public void Dispose()
        {
            foreach (var boundary in m_Boundaries)
                boundary.EntityEntered -= OnBoundaryEnter;
        }

        public float[] GetBoundaries()
        {
            return new float[4] { m_BoundarySizeTop, m_BoundarySizeBottom, m_BoundarySizeLeft, m_BoundarySizeRight };
        }

        

        private void OnBoundaryEnter(IBoundary mapBoundary, IEntity entity)
        {
            //Debug.Log($"{entity.GetName()} entered {mapBoundary.Index.GetName()}");

            switch (mapBoundary.Index)
            {
                default : break;

                case BoundaryIndex.Top:
                        entity.SetPosition(new Vector3(entity.Position.x, m_BoundarySizeTop, entity.Position.z));
                    TopBoundaryReached?.Invoke(entity);
                    break;

                case BoundaryIndex.Bottom:
                        entity.SetPosition(new Vector3(entity.Position.x, m_BoundarySizeBottom, entity.Position.z));
                    BottomBoundaryReached?.Invoke(entity);
                    break;

                case BoundaryIndex.Left:
                        entity.SetPosition(new Vector3(m_BoundarySizeRight, entity.Position.y, entity.Position.z));
                    LeftBoundaryReached?.Invoke(entity);
                    break;

                case BoundaryIndex.Right:
                        entity.SetPosition(new Vector3(m_BoundarySizeLeft, entity.Position.y, entity.Position.z));
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
            m_MapUI.SetSize(m_Width, m_Hight);
        }
    
    
    }

    public struct MapConfig : IConfig
    {
        public int Hight { get; private set; }
        public int Width { get; private set; }
        public IBoundary[] Boundaries { get; private set; }

        public MapConfig(int hight, int width, IBoundary[] boundaries)
        {
            Hight = hight;
            Width = width;
            Boundaries = boundaries;
        }
    }

}

namespace APP
{
    public interface IMap
    {
        int Hight { get; }
        int Width { get; }

        float[] GetBoundaries();
    }
}