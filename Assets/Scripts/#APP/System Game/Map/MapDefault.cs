using System;
using UnityEngine;

namespace APP.Game.Map
{
    public class MapDefault : MapModel<MapDefault>, IMap
    {
        
        [SerializeField] private float m_Width;
        [SerializeField] private float m_Hight;
        
        [SerializeField] private MapBoundary m_BoundaryTop;
        [SerializeField] private MapBoundary m_BoundaryBottom;
        [SerializeField] private MapBoundary m_BoundaryLeft;
        [SerializeField] private MapBoundary m_BoundaryRight;
        
        
        public MapDefault() { }
        public MapDefault(params object[] args) => Configure(args);

        public override void Configure(params object[] args)
        {
            //var width = 250;
            //var hight = 100;
            



            var config = new MapConfig(m_Width, m_Hight);
            base.Configure(config);
        }

    }

    public class MapModel<TMap> : MonoBehaviour, IConfigurable
    {
        [SerializeField] private MapBoundaryController m_MapBoundaryController;
        [SerializeField] private MapUI m_MapUI;
        

        [SerializeField] private float m_HightWater;
        [SerializeField] private float m_HightGround;
        

        private MapConfig m_Config;
        

        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        
        public float Width => m_Config.Width;
        public float Hight => m_Config.Hight;
        public float HightWater => m_HightWater;
        public float HightGround => m_HightGround;

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;



        public virtual void Configure(params object[] args)
        {
            m_Config = (MapConfig) args[0];
            
            m_HightWater = m_Config.Hight - 15;
            m_HightGround = m_Config.Hight - 85;

        }

        public void Init()
        {
            //m_MapUI.SetSize(m_Config.Width, m_Config.Hight);
            //m_MapBoundaryController.SetBoundaries(m_Config.Width, m_Config.Hight);
            
        }

        public void Dispose()
        {

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
            
        }
    
    
    }

    public struct MapConfig : IConfig
    {
        public float Width { get; private set; }
        public float Hight { get; private set; }
        

        public MapConfig(float hight, float width)
        {
            Hight = hight;
            Width = width;
        }
    }

}

namespace APP
{
    public interface IMap
    {
        float Width { get; }
        float Hight { get; }
        float HightWater { get; }
        float HightGround { get; }

    }
}