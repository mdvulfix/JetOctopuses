using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Transform))]
    public abstract class PlayerModel<TPlayer> : MonoBehaviour
    {


        [Header("UI")]
        [SerializeField] private PlayerUI m_PlayerUI;

        [Header("Components")]
        [SerializeField] private ControlZone m_Vision;
        [SerializeField] private ControlZone m_AttackZone;
        [SerializeField] private ControlZone m_EatZone;

        [Header("Stats")]
        [SerializeField] private float m_Health;
        [SerializeField] private float m_Energy;
        [SerializeField] private float m_EnergyMax = 100;
        [SerializeField] private float m_Score;
        [SerializeField] private float m_Speed;


        private Rigidbody2D m_Rigidbody;
        private Collider2D m_Collider;
        private Transform m_Transform;


        private IMoveBehaviour m_Move;
        private IEatBehaviour m_Eat;
        private IAttackBehaviour m_Attack;
        private IDamageBehaviour m_Damage;
        private IDieBehaviour m_Die;

        private List<IEntity> m_EntiesInEatRange;
        private List<IEntity> m_EntiesInAttackRange;
        private List<IEntity> m_EntiesInVisionkRange;

        public Vector3 Position => transform.position;



        public event Action<IEntity> FoodWasConsumed;
        public event Action<IEntity> EntityAttacked;



        public virtual void Configure(params object[] args)
        {

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Collider = GetComponent<Collider2D>();
            m_Transform = GetComponent<Transform>();


            m_Speed = 3f;


            m_Move = new BehaviourMovePlayer(m_Rigidbody, m_Speed);

            m_EntiesInEatRange = new List<IEntity>();
            m_Eat = new BehaviourEatPlayer(m_EntiesInEatRange);

            m_EntiesInAttackRange = new List<IEntity>();
            m_Attack = new BehaviourAttackPlayer(m_EntiesInAttackRange, () => Position);


            m_EntiesInVisionkRange = new List<IEntity>();

        }

        public virtual void Init()
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

        public virtual void Dispose()
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



        public void Move() =>
            m_Move.Do();

        public void Eat() =>
            m_Eat.Do();

        public void Attack() =>
            m_Attack.Do();

        public void Damage(float damage) =>
            m_Damage.Do();

        public void Die() =>
            m_Die.Do();


        public void SetPosition(Vector3 position) =>
            m_Transform.position = position;

        public void AddForce(Vector3 direction) =>
            m_Rigidbody.AddForce(direction, ForceMode2D.Impulse);


        // VISION AND CONTROL //
        private void OnEntityInEatZone(IEntity entity)
        {
            if (entity is IFood)
            {
                m_EntiesInEatRange.Add(entity);
                // Debug.Log($"{entity.GetName()} enter eat zone.");
            }
        }

        private void OnEntityOutEatZone(IEntity entity)
        {
            if (entity is IFood)
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
            if (entity is IEnemy)
            {
                m_EntiesInAttackRange.Add(entity);
                Debug.Log($"{entity.GetName()} enter attack zone.");
            }
        }

        private void OnEntityOutAttackZone(IEntity entity)
        {
            if (entity is IEnemy)
            {
                try
                {
                    m_EntiesInAttackRange.Remove(entity);
                }
                catch (System.Exception)
                {
                    Debug.Log($"{entity.GetName()} is not found!");
                }

                Debug.Log($"{entity.GetName()} exit attack zone.");
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



        // EAT //
        private void OnFoodWasConsumed(IEntity food)
        {
            OnEntityOutEatZone(food);
            OnEntityOutVisionZone(food);

            FoodWasConsumed?.Invoke(food);
        }

        private void OnEnergyWasted(float energy)
        {
            if (m_Energy - energy < 0)
                m_Energy = 0;
            else
                m_Energy -= energy;
        }

        private void OnEnergyReceived(float energy)
        {
            if (m_Energy + energy > m_EnergyMax)
                m_Energy = m_EnergyMax;
            else
                m_Energy += energy;
        }

        // ATTACK //
        private void OnEntityAttacked(float damage, IEntity entity)
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

}


namespace App
{
    public interface IPlayer : IEntity
    {
        void Move();
        void Eat();
        void Attack();
        void Damage(float damage);
        void Die();

    }
}