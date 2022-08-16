using System;
using System.Collections.Generic;
using UnityEngine;

namespace APP.Game
{
    public abstract class PlayerModel<TPlayer> : MonoBehaviour
    {
        

        [Header("UI")]
        [SerializeField] private PlayerUI m_PlayerUI;
        
        [Header("Components")]
        [SerializeField] private ControlZone m_PlayerVision;
        [SerializeField] private ControlZone m_PlayerAttackZone;
        [SerializeField] private ControlZone m_PlayerEatZone;

        [Header("Stats")]
        [SerializeField] private float m_Health;
        [SerializeField] private float m_Energy;
        [SerializeField] private float m_EnergyMax = 100;
        [SerializeField] private float m_Score;
        [SerializeField] private float m_Speed;


        private IBehaviour m_MoveBehaviour;
        private IEatBehaviour m_EatBehaviour;
        private IAttackBehaviour m_AttackBehaviour;

        private List<IEntity> m_EntiesInEatRange;
        private List<IEntity> m_EntiesInAttackRange;
        private List<IEntity> m_EntiesInVisionkRange;

        public Vector3 Position => transform.position;

        
        
        public virtual void Configure(params object[] args)
        {
            m_Speed = 2f;
            var rigidbody2D =  GetComponent<Rigidbody2D>();
            m_MoveBehaviour = new BehaviourMovePlayer(rigidbody2D, m_Speed);

  
            m_EntiesInEatRange = new List<IEntity>();
            m_EatBehaviour = new BehaviourEatPlayer(m_EntiesInEatRange);

            m_EntiesInAttackRange = new List<IEntity>();
            m_AttackBehaviour = new BehaviourAttackPlayer(m_EntiesInAttackRange);

            m_EntiesInVisionkRange = new List<IEntity>();

        }
        
        
        public virtual void Init()
        {
            m_PlayerVision.InZone += OnEntityInVisionZone;
            m_PlayerVision.OutZone += OnEntityOutVisionZone;

            m_PlayerAttackZone.InZone += OnEntityInAttackZone;
            m_PlayerAttackZone.OutZone += OnEntityOutAttackZone;

            m_PlayerEatZone.InZone += OnEntityInEatZone;
            m_PlayerEatZone.OutZone += OnEntityOutEatZone;

            m_EatBehaviour.EnergyReceived += OnEnergyReceived;
            m_EatBehaviour.EnergyWasted += OnEnergyWasted;
            m_EatBehaviour.FoodWasConsumed += OnFoodWasConsumed;

        }



        public virtual void Dispose()
        {
            m_PlayerVision.InZone -= OnEntityInVisionZone;
            m_PlayerVision.OutZone -= OnEntityOutVisionZone;

            m_PlayerAttackZone.InZone -= OnEntityInAttackZone;
            m_PlayerAttackZone.OutZone -= OnEntityOutAttackZone;

            m_PlayerEatZone.InZone -= OnEntityInEatZone;
            m_PlayerEatZone.OutZone -= OnEntityOutEatZone;

            m_EatBehaviour.EnergyReceived -= OnEnergyReceived;
            m_EatBehaviour.EnergyWasted -= OnEnergyWasted;
            m_EatBehaviour.FoodWasConsumed -= OnFoodWasConsumed;
        }
        
        
        public void Move() =>
            m_MoveBehaviour.Do();

        public void Eat() =>
            m_EatBehaviour.Do();

        public void Attack() =>
            m_AttackBehaviour.Do();

        public void SetPosition(Vector3 position) => 
            transform.position = position;


        private void OnEntityInEatZone(IEntity entity)
        {
            if(entity is IFood)
            {
                m_EntiesInEatRange.Add(entity);
                Debug.Log($"{entity.GetName()} enter eat zone.");
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

                Debug.Log($"{entity.GetName()} exit eat zone.");
            }

                
        }

        private void OnEntityInAttackZone(IEntity entity)
        {
            if(entity is IEnemy)
            {
                m_EntiesInAttackRange.Add(entity);
                Debug.Log($"{entity.GetName()} enter attack zone.");
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

                Debug.Log($"{entity.GetName()} exit attack zone.");
            }
        }

        private void OnEntityInVisionZone(IEntity entity)
        {

            m_EntiesInVisionkRange.Add(entity);
            Debug.Log($"{entity.GetName()} enter vision zone.");

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

            Debug.Log($"{entity.GetName()} exit vision zone.");
        }

        private void OnFoodWasConsumed(IFood food)
        {
            OnEntityOutEatZone(food);
            OnEntityOutVisionZone(food);

            if(food is MonoBehaviour)
                Destroy(((MonoBehaviour)food).gameObject);
            
        }

        private void OnEnergyWasted(float energy)
        {
            if(m_Energy - energy < 0)
                m_Energy = 0;
            else
                m_Energy -= energy;
        }

        private void OnEnergyReceived(float energy)
        {
            if(m_Energy + energy > m_EnergyMax)
                m_Energy = m_EnergyMax;
            else
                m_Energy += energy;
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


namespace APP
{
    public interface IPlayer: IEntity
    {
        void Eat();
        void Attack();
        void Move();
    }
}