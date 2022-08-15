using System;
using System.Collections.Generic;
using UnityEngine;

namespace APP.Game
{

    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyModel<TEnemy> : MonoBehaviour
    {
        [SerializeField] private float m_Energy;
        [SerializeField] private float m_EnergyMin;

        private List<IFood> m_Food;

        private EnemyState m_StateActive;

        public string Name { get; private set; }
        public float Health { get; private set; }
        public float Energy => m_Energy;

        public event Action Dead;

        public virtual void Damage() { }
        
        private IBehaviour m_MoveBehaviour;
        private IBehaviour m_EatBehaviour;
        private IBehaviour m_ChaseBehaviour;
        private IAttackBehaviour m_AttackBehaviour;
        private IBehaviour m_RoamBehaviour;


        public virtual void Eat() =>
            m_EatBehaviour.Do();

        public virtual void Chase() =>
            m_ChaseBehaviour.Do();

        public virtual void Attack() =>
            m_AttackBehaviour.Do();

        public virtual void Roam()
        {
            m_RoamBehaviour.Do();
            CalculateEnergy();
        }

        public virtual void Die() 
        {
            Dead?.Invoke();
        }


        private void CalculateEnergy()
        {
            m_Energy -= Time.deltaTime;
            
            if(m_Energy <= m_EnergyMin)
                m_StateActive = EnemyState.Eat;
            
            if (m_Energy <= 0)
            { 
                m_Energy = 0;
                m_StateActive = EnemyState.Die;
            }
            
        }


        private void Awake()
        {
            Name = gameObject.name;
            
            m_Energy = 100f;
            m_EnergyMin = 25f;

            var rigidbody = GetComponent<Rigidbody2D>();
            var moveSpeed = 2;
            var roamDistance = 4;
            var roamStartPosition = transform.position;

            m_MoveBehaviour = new BehaviourMoveAI(rigidbody, moveSpeed, roamStartPosition, roamDistance);
            m_EatBehaviour = new BehaviourEatAI();
            m_ChaseBehaviour= new BehaviourChaseAI();
            //m_AttackBehaviour = new BehaviourAttackAI();

            m_StateActive = EnemyState.Roam;
        }

        private void OnEnable() 
        {
            m_AttackBehaviour.EnemyAttacked += OnEnemyAttack;
            m_AttackBehaviour.EnergyWasted += OnEnergyWasted;
        }

        private void OnDisable() 
        {
            m_AttackBehaviour.EnemyAttacked -= OnEnemyAttack;
            m_AttackBehaviour.EnergyWasted -= OnEnergyWasted;
        }




        private void FixedUpdate()
        {
            HandleState();
        }

        private void HandleState()
        {
            switch (m_StateActive)
            {
                default: break;

                case EnemyState.Roam:
                    Roam();
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


        private void OnEnemyAttack(float damage, IEnemy enemy)
        { 

        }

        private void OnEnergyWasted(float energy)
        { 

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
    public interface IEnemy
    {
        string Name { get; }
        
        void Roam();
        void Eat();
        void Chase();
        void Attack();
        void Die();
    }
}