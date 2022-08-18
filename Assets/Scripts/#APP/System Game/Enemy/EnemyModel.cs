using System;
using System.Collections.Generic;
using UnityEngine;

namespace APP.Game
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Transform))]
    public abstract class EnemyModel<TEnemy> : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private EnemyUI m_UI;

        [Header("Components")]
        [SerializeField] private ControlZone m_Vision;
        [SerializeField] private ControlZone m_AttackZone;
        [SerializeField] private ControlZone m_EatZone;

        [Header("Stats")]
        [SerializeField] private float m_Health;
        [SerializeField] private float m_HealthMax = 100;

        [SerializeField] private float m_Energy;
        [SerializeField] private float m_EnergyMax = 100;
        [SerializeField] private float m_EnergyMin = 25;

        [SerializeField] private float m_Speed;
        [SerializeField] private float m_LowEnergySpeedModifier = 0.25f;
        [SerializeField] private float m_LowEnergyRoamDistanseModifier = 0.15f;

        [Header("State")]
        [SerializeField] private EnemyState m_StateActive;

        private EnemyConfig m_Config;
        private IEnemy m_Enemy;
        
        private Rigidbody2D m_Rigidbody;
        private Collider2D m_Collider;
        private Transform m_Transform;

        private IMoveBehaviour m_Move;
        private IEatBehaviour m_Eat;
        private IChaseBehaviour m_Chase;
        private IAttackBehaviour m_Attack;

        private List<IEntity> m_EntiesInEatRange;
        private List<IEntity> m_EntiesInAttackRange;
        private List<IEntity> m_EntiesInVisionkRange;
        

        public float Health => m_Health;
        public float Energy => m_Energy;
        public Vector3 Position => transform.position;

        public event Action<IEntity> Dead;

        public virtual void Configure(params object[] args)
        {
            if(args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if(arg is EnemyConfig)
                    {
                        m_Config = (EnemyConfig) args[0];
                        m_Enemy = m_Config.Enemy;
                    }

                }
            }

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Collider = GetComponent<Collider2D>();
            m_Transform = GetComponent<Transform>();

            m_Health = 100;
            m_Energy = 100;
            m_Speed = 2;

            m_Move = new BehaviourMoveAI(m_Rigidbody, m_Speed, m_Transform.position, m_Vision.Radius);

            m_EntiesInVisionkRange = new List<IEntity>();
            //m_Roam = new BehaviourRoamAI(m_EntiesInVisionkRange, m_Move);
  
            m_EntiesInVisionkRange = new List<IEntity>();
            //m_Chase = new BehaviourChaseAI(m_EntiesInVisionkRange, m_Move);
            
            m_EntiesInAttackRange = new List<IEntity>();
            m_Attack = new BehaviourAttackAI(m_EntiesInAttackRange);

            m_EntiesInEatRange = new List<IEntity>();
            m_Eat = new BehaviourEatAI(m_EntiesInEatRange);

        }

        public virtual void Init()
        {
            Subscribe();

            m_StateActive = EnemyState.Roam;

            m_UI.Configure();
            m_UI.Init();

        }

        public virtual void Dispose()
        {
            m_UI.Dispose();
            
            Unsubscribe();
        }


        public void Subscribe()
        { 
            m_Vision.InZone += OnEntityInVisionZone;
            m_Vision.OutZone += OnEntityOutVisionZone;

            m_AttackZone.InZone += OnEntityInAttackZone;
            m_AttackZone.OutZone += OnEntityOutAttackZone;

            m_EatZone.InZone += OnEntityInEatZone;
            m_EatZone.OutZone += OnEntityOutEatZone;

            m_Eat.EnergyReceived += OnEnergyReceived;
            m_Eat.EnergyWasted += OnEnergyWasted;
            m_Eat.FoodWasConsumed += OnFoodWasConsumed;

            m_Attack.EnergyWasted += OnEnergyWasted;
            m_Attack.EntityAttacked += OnEntityAttacked;
        }

        public void Unsubscribe()
        { 
            m_Vision.InZone -= OnEntityInVisionZone;
            m_Vision.OutZone -= OnEntityOutVisionZone;

            m_AttackZone.InZone -= OnEntityInAttackZone;
            m_AttackZone.OutZone -= OnEntityOutAttackZone;

            m_EatZone.InZone -= OnEntityInEatZone;
            m_EatZone.OutZone -= OnEntityOutEatZone;

            m_Eat.EnergyReceived -= OnEnergyReceived;
            m_Eat.EnergyWasted -= OnEnergyWasted;
            m_Eat.FoodWasConsumed -= OnFoodWasConsumed;

            m_Attack.EnergyWasted += OnEnergyWasted;
            m_Attack.EntityAttacked += OnEntityAttacked;
        }

        

        
        
        
        public virtual void Eat() => 
            m_Eat.Do();

        public virtual void Chase() =>
            m_Chase.Do();

        public virtual void Attack() =>
            m_Attack.Do();

        public virtual void Move() =>
            m_Move.Do();

        public virtual void Damage(float damage)
        {
            m_Health -= damage;
            m_UI.PopupShowDamage((int)damage);
            Debug.Log("Damage done " +  damage);
        }

        public virtual void Die()
        {
            m_StateActive = EnemyState.None;
            Dead?.Invoke(m_Enemy);
        }
       
        
        public void SetPosition(Vector3 position) =>
            m_Transform.position = position;

        public void AddForce(Vector3 direction) => 
            m_Rigidbody.AddForce(direction, ForceMode2D.Impulse);

        
        
        
        
        
        private void HandleState()
        {
            switch (m_StateActive)
            {
                default : break;

                case EnemyState.Roam:
                        Move();
                    break;

                case EnemyState.Eat:
                        Eat();
                    break;

                case EnemyState.Chase:
                        Chase();
                    break;

                case EnemyState.Attack:
                        Attack();
                    break;

                case EnemyState.Die:
                        Die();
                    break;

            }

        }

        private void EnergyCalculate()
        {
            m_Energy -= Time.deltaTime;

            if (m_Energy <= m_EnergyMin)
                m_StateActive = EnemyState.Eat;

            if (m_Energy <= 0)
            {
                m_Energy = 0;
                if(m_Move.Modified == false)
                {
                    var lowEnergyModifier = new BehaviourMoveModifyer(m_LowEnergySpeedModifier, m_LowEnergyRoamDistanseModifier);
                    m_Move.Modify(lowEnergyModifier);
                }
            }
            else
            {
                if(m_Move.Modified == true)
                    m_Move.Default();
            }

        }

        private void HealthCalculate()
        {
            if(m_Energy <= 0 && m_Health > 0)
                m_Health -= Time.deltaTime;
            
            if (m_Health <= 0)
            {
                m_Health = 0;
                m_StateActive = EnemyState.Die;
            }

        }

 
        // VISION AND CONTROL //
        private void OnEntityInEatZone(IEntity entity)
        {
            if(entity is IFood)
            {
                m_EntiesInEatRange.Add(entity);
                //Debug.Log($"{entity.GetName()} enter eat zone.");
            }     
        }

        private void OnEntityOutEatZone(IEntity entity)
        {
            if(entity is IFood)
            {
                try
                {
                    m_EntiesInEatRange.Remove(entity);
                }
                catch (System.Exception)
                {
                    Debug.Log($"{entity.GetName()} is not found!");
                }

                //Debug.Log($"{entity.GetName()} exit eat zone.");
            }

                
        }

        private void OnEntityInAttackZone(IEntity entity)
        {
            if(entity is IPlayer)
            {
                m_EntiesInAttackRange.Add(entity);
                //Debug.Log($"{entity.GetName()} enter attack zone.");
            }   
        }

        private void OnEntityOutAttackZone(IEntity entity)
        {
            if(entity is IEnemy)
            {
                try
                {
                    m_EntiesInAttackRange.Remove(entity);
                }
                catch (System.Exception)
                {
                    Debug.Log($"{entity.GetName()} is not found!");
                }

                //Debug.Log($"{entity.GetName()} exit attack zone.");
            }
        }

        private void OnEntityInVisionZone(IEntity entity)
        {

            m_EntiesInVisionkRange.Add(entity);
            //Debug.Log($"{entity.GetName()} enter vision zone.");

        }

        private void OnEntityOutVisionZone(IEntity entity)
        {
            try
            {
                m_EntiesInVisionkRange.Remove(entity);
            }
            catch (System.Exception)
            {
                Debug.Log($"{entity.GetName()} is not found!");
            }

            //Debug.Log($"{entity.GetName()} exit vision zone.");
        }

        
        private void OnFoodWasConsumed(IEntity entity)
        {

        }

        private void OnEnergyReceived(float energy)
        {

        }

        private void OnEntityAttacked(float damage, IEntity enemy)
        {

        }

        private void OnEnergyWasted(float energy)
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
            EnergyCalculate();
            HealthCalculate();
            
            HandleState();
        }

        private void FixedUpdate()
        {
            
        }


    }

    public class EnemyConfig
    {
        public IEnemy Enemy { get; private set; }

        public EnemyConfig(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }

    public enum EnemyState
    {
        None,
        Roam,
        Eat,
        Chase,
        Attack,
        Die
    }

}

namespace APP
{
    public interface IEnemy : IEntity
    {
        void Move();
        void Eat();
        void Chase();
        void Attack();
        void Damage(float damage);
        void Die();
    }
}