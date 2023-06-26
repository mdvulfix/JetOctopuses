using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

//using App.Camera;
using App.Game.Map;
using System;
/*
namespace App.Game
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

        [SerializeField] private PlayerController m_PlayerController;

        [SerializeField] private EnemyDefault[] m_Enemies;


        private EnemySpawner m_EnemySpawner;
        private List<IEnemy> m_Enemy;

        private FoodSpawner m_FoodSpawner;
        private List<IFood> m_Food;


        public IMap Map => m_Map;

        public ICamera CameraFollowPlayer => m_Camera;
        public IPlayer PLayer => m_Player;

        public virtual void Configure(params object[] args)
        {
            m_EnemySpawner = new EnemySpawner(m_EnemySprite, ROOT_ENEMY);
            m_FoodSpawner = new FoodSpawner(m_FoodSprite, ROOT_FOOD);
        }


        public virtual void Init()
        {
            //var enemyQuantity = 5;
            //SpawnEnemy(enemyQuantity);

            var foodNamber = 25;
            m_Food = new List<IFood>();

            for (int i = 0; i < foodNamber; i++)
                SpawnFood(GetRendomPosition());


            m_CameraZoom = 5f;
            m_CameraZoomSensitivity = 10f;

            var position = new Vector2(0, 0);
            var fieldOfView = 40;
            var cameraConfig = new CameraConfig(position, fieldOfView);
            m_Camera.Configure(cameraConfig);
            m_Camera.Follow(() => m_Player.transform.position);
            m_Camera.Zoom(() => m_CameraZoom);

            m_PlayerController.FoodWasConsumed += OnFoodWasConsumed;
            m_PlayerController.EntityAttacked += OnEntityAttacked;

            if (m_Enemies.Length > 0)
                foreach (var enemy in m_Enemies)
                    enemy.Dead += OnEnemyDead;

        }


        public virtual void Dispose()
        {
            m_Player.FoodWasConsumed -= OnFoodWasConsumed;
            m_Player.EntityAttacked -= OnEntityAttacked;

            if (m_Enemies.Length > 0)
                foreach (var enemy in m_Enemies)
                    enemy.Dead -= OnEnemyDead;
        }


        private IFood SpawnFood(Vector3 position)
        {
            if (m_Food == null)
                m_Food = new List<IFood>();

            var food = m_FoodSpawner.Spawn(position);
            m_Food.Add(food);

            return food;
        }

        private void SpawnEnemy(int quantity = 1)
        {
            if (m_Enemy == null)
                m_Enemy = new List<IEnemy>();

            for (int i = 0; i < quantity; i++)
                m_Enemy.Add(m_EnemySpawner.Spawn(GetRendomPosition()));
        }

        private Vector3 GetRendomPosition() =>
            new Vector3(URandom.Range(-Map.Width / 2, Map.Width / 2 + 1), URandom.Range(-Map.Hight / 2, Map.Hight / 2 + 1), 0);


        private void OnBoundaryReached(Collider2D collider)
        {
            if (collider.TryGetComponent<IEnemy>(out var entity))
            {

            }
        }

        // FOOD: POOL OR DESTROY //
        private void OnFoodWasConsumed(IEntity food)
        {
            if (food is MonoBehaviour)
                Destroy(((MonoBehaviour)food).gameObject);

        }

        // ENEMY: POOL OR DESTROY //
        private void OnEnemyDead(IEntity enemy)
        {
            var position = enemy.Position;

            if (enemy is MonoBehaviour)
                Destroy(((MonoBehaviour)enemy).gameObject);

            var foodNamber = 5;
            for (int i = 0; i < foodNamber; i++)
            {
                var food = SpawnFood(position);
                var direction = new Vector3(URandom.Range(-5, 6) * 0.5f, URandom.Range(-5, 6) * 0.5f, 0);
                food.AddForce(direction);
            }

        }


        private void OnEntityAttacked(IEntity enemy)
        {

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


        }

        private void Update()
        {
            m_CameraZoom += Input.GetAxis("Mouse ScrollWheel") * m_CameraZoomSensitivity * -1;

            //var zoomDelta = 2f + Input.mouseScrollDelta.y;
            // constrain zoom
            m_CameraZoom = Mathf.Clamp(m_CameraZoom, 5f, 15f);
            // apply zoom to field of view

        }

    }
}
*/