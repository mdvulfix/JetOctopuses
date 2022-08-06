using System;
using UnityEngine;

namespace APP.Map
{
    public class MapDefault : MapModel<MapDefault>
    {
        public MapDefault() { }
        public MapDefault(params object[] args)
        => Configure(args);


        public override void Configure(params object[] args)
        {
            var hight = 25;
            var width = 25;
            var config = new MapConfig(hight, width);

            base.Configure(config);
        }

    }


    public class MapModel<TMap> : MonoBehaviour, IConfigurable
    {
        [SerializeField] private int m_Hight;
        [SerializeField] private int m_Width;

        private MapConfig m_Config;

        private BoxCollider2D m_Collider;
        
        [SerializeField] private float m_BoundaryTop;
        [SerializeField] private float m_BoundaryBottom;
        [SerializeField] private float m_BoundaryLeft;
        [SerializeField] private float m_BoundaryRight;
        [SerializeField] private float m_BoundaryHightOffset;
        [SerializeField] private float m_BoundaryWidthOffset;


        public bool IsConfigured { get; private set; }
        public bool IsInitialized { get; private set; }

        public event Action Configured;
        public event Action Initialized;
        public event Action Disposed;


        public virtual void Configure(params object[] args)
        {
            m_Config = (MapConfig)args[0];
            m_Hight = m_Config.Hight;
            m_Width = m_Config.Width;

            m_BoundaryHightOffset = -2;
            m_BoundaryWidthOffset = 0;


            m_Collider = gameObject.AddComponent<BoxCollider2D>();
            m_Collider.isTrigger = true;
            m_Collider.size = new Vector2(m_Width - 1, m_Hight - 5);
            m_Collider.offset = new Vector2(m_BoundaryWidthOffset, m_BoundaryHightOffset);

            m_BoundaryTop = m_Collider.size.y / 2 + m_Collider.offset.y;
            m_BoundaryBottom = - (m_Collider.size.y / 2) + m_Collider.offset.y;
            m_BoundaryLeft = - (m_Collider.size.x / 2) + m_Collider.offset.x;
            m_BoundaryRight = m_Collider.size.x / 2 + m_Collider.offset.x;


        }

        public void Init()
        {

        }

        public void Dispose()
        {

        }




        private void Awake()
        {
            Configure();

        }

        private void FixedUpdate()
        {
            OnTriggerExit2D(m_Collider);

        }
        
        
        
        private void OnTriggerExit2D(Collider2D collider) 
        {


            Debug.Log($"Position: {collider.transform.position}");

            if (collider.transform.position.x >= m_BoundaryRight)
            { 
                collider.transform.position = new Vector3(m_BoundaryLeft, collider.transform.position.y, collider.transform.position.z);
                Debug.Log($"Position (new): {collider.transform.position}");
            }
            else if (collider.transform.position.x <= m_BoundaryLeft)
            { 
                collider.transform.position = new Vector3(m_BoundaryRight, collider.transform.position.y, collider.transform.position.z);
                Debug.Log($"Position (new): {collider.transform.position}");
            }
            else if (collider.transform.position.y >= m_BoundaryTop)
            { 
                collider.transform.position = new Vector3(collider.transform.position.x, m_BoundaryBottom, collider.transform.position.z);
                Debug.Log($"Position (new): {collider.transform.position}");
            }
            else if (collider.transform.position.y <= m_BoundaryBottom)
            { 
                collider.transform.position = new Vector3(collider.transform.position.x, m_BoundaryTop, collider.transform.position.z);
                Debug.Log($"Position (new): {collider.transform.position}");
            }

            //var freezY = collider.transform.position.y;

            //collider.transform.position = new Vector3(position.x, freezY, position.z);

        }


    }

    public struct MapConfig : IConfig
    {
        public int Hight { get; private set; }
        public int Width { get; private set; }

        public MapConfig(int hight, int width)
        {
            Hight = hight;
            Width = width;
        }
    }

}

