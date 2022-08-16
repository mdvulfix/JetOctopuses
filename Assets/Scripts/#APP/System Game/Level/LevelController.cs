using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

using APP.Camera;
using APP.Game.Map;

namespace APP.Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameObject ROOT_FOOD;
        [SerializeField] private GameObject ROOT_ENEMY;
        [SerializeField] private GameObject ROOT_PLAYER;
        [SerializeField] private GameObject ROOT_MAP;

        [SerializeField] private MapDefault m_Map;
        
        [SerializeField] private CameraFollowPlayer m_Camera;
        [SerializeField] private PlayerDefault m_Player;

        [SerializeField] private float m_CameraZoom;
        [SerializeField] private float m_CameraZoomSensitivity;

        [SerializeField] private Sprite m_FoodSprite;
        [SerializeField] private Sprite m_EnemySprite;

        private EnemySpawner m_EnemySpawner;
        private List<IEnemy> m_Enemy;

        private FoodSpawner m_FoodSpawner;
        private List<IFood> m_Food;
        
        
        public IMap Map => m_Map;

        public ICamera CameraFollowPlayer => m_Camera;
        public IPlayer PLayer => m_Player;

        
        private void Awake() 
        {
            
            m_EnemySpawner = new EnemySpawner(m_EnemySprite, ROOT_ENEMY);
            m_FoodSpawner = new FoodSpawner(m_FoodSprite, ROOT_FOOD);

        }
        
        private void OnEnable() 
        {
            
        }
        
        private void OnDisable() 
        {
            
        }
        
        private void Start() 
        {
            var enemyQuantity = 5;
            SpawnEnemy(enemyQuantity);

            var foodQuantity = 15;
            m_Food = new List<IFood>();
            SpawnFood(foodQuantity);

            
            m_CameraZoom = 5f;
            m_CameraZoomSensitivity = 10f;

            var position = new Vector2(0, 0);
            var fieldOfView = 40;
            var cameraConfig = new CameraConfig(position, fieldOfView);
            m_Camera.Configure(cameraConfig);
            m_Camera.Follow(() => m_Player.transform.position);
            m_Camera.Zoom(() => m_CameraZoom);

        }

        private void Update() 
        {
            m_CameraZoom += Input.GetAxis("Mouse ScrollWheel") * m_CameraZoomSensitivity * -1;

            //var zoomDelta = 2f + Input.mouseScrollDelta.y;
            // constrain zoom
            m_CameraZoom = Mathf.Clamp(m_CameraZoom, 5f, 15f);
            // apply zoom to field of view

        }


        private void SpawnFood(int quantity = 1)
        { 
            if(m_Food == null)
                m_Food = new List<IFood>();

            for (int i = 0; i < quantity; i++)
                m_Food.Add(m_FoodSpawner.Spawn(GetRendomPosition()));

        }
        
        private void SpawnEnemy(int quantity = 1)
        { 
            if(m_Enemy == null)
                m_Enemy = new List<IEnemy>();
            
            for (int i = 0; i < quantity; i++)
                m_Enemy.Add(m_EnemySpawner.Spawn(GetRendomPosition()));
        }

        private Vector3 GetRendomPosition() =>
            new Vector3(URandom.Range(-Map.Width/2, Map.Width/2 +1), URandom.Range(-Map.HightWater/2, Map.HightWater/2 +1), 0);

    
        private void OnBoundaryReached(Collider2D collider)
        {
            if (collider.TryGetComponent<IEnemy>(out var entity))
            { 
                
            }
        }
    
    }
}