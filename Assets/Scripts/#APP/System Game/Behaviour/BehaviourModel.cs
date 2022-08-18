using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace APP.Game
{
    // MOVE //
    public class BehaviourMovePlayer : BehaviourModel, IMoveBehaviour
    {
        public BehaviourMovePlayer() { }
        public BehaviourMovePlayer(Rigidbody2D rigidbody, float speed)
        {
            Speed = speed;
            Rigidbody = rigidbody;
        }

        
        public float Speed { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }

        public event Action<float> EnergyWasted;

        public override void Do()
        {
            Rigidbody.velocity = CalculateDirection();
        }

        private Vector3 CalculateDirection()
        {
            var moveHorizontal = 0;
            var moveVertical = 0;

            if (Input.GetKey(KeyCode.W))
                moveVertical = 1;

            if (Input.GetKey(KeyCode.S))
                moveVertical = -1;

            if (Input.GetKey(KeyCode.A))
                moveHorizontal = -1;

            if (Input.GetKey(KeyCode.D))
                moveHorizontal = 1;

            return new Vector3(moveHorizontal, moveVertical, 0).normalized * Speed;
        }
    }

    public class BehaviourMoveAI : BehaviourModel, IMoveBehaviour
    {
        private Rigidbody2D m_Rigidbody;
        
        private float m_Speed;
        private float m_SpeedDefault;
        
        private Vector3 m_RoamStartPosition;
        private float m_RoamDistance;
        private float m_RoamDistanceDefault;

        private float m_CooldownTime = 1f;
        private float m_CooldownTimeMax = 5f;
        
        public BehaviourMoveAI() { }
        public BehaviourMoveAI(
            Rigidbody2D rigidbody,
            float speed,
            Vector2 roamStartPosition,
            float roamDistance)
        {
            m_Speed = speed;
            m_SpeedDefault = speed;

            m_RoamStartPosition = roamStartPosition;
            m_RoamDistance = roamDistance;
            m_RoamDistanceDefault = roamDistance;
            
            m_Rigidbody = rigidbody;

            Modified = false;
        }


        public event Action<float> EnergyWasted;
        

        public override void Do()
        {
            if (Cooldown() == false)
                return;

            m_Rigidbody.velocity = GetDirection();

        }

        public override void Modify(IBehaviourModifier modifier)
        {
            var moveModifyer = (BehaviourMoveModifyer) modifier;
            
            m_Speed *=  moveModifyer.SpeedModifier;
            m_RoamDistance *=  moveModifyer.RoamDistanceModifier;

            Modified = true;
        }

        public override void Default()
        {
            m_Speed = m_SpeedDefault;
            m_RoamDistance = m_RoamDistanceDefault;

            Modified = false;
        }



        private Vector3 GetDirection()
        {
            var moveHorizontal = URandom.Range(-1, 2);
            var moveVertical = URandom.Range(-1, 2);

            return new Vector3(moveHorizontal, moveVertical, 0).normalized * m_Speed;
        }
        
        private float GetDistance()
        {
            return Vector3.Distance(m_Rigidbody.transform.position, m_RoamStartPosition);
        }

        private bool Cooldown()
        {
            m_CooldownTime -= Time.deltaTime;
            
            //if(m_RoamDistance <= GetDistance())
            //    m_CooldownTime = 0;
            
            if (m_CooldownTime <= 0)
            {
                m_CooldownTime = URandom.Range(0, m_CooldownTimeMax + 1f);
                return true;
            }

            return false;
        }

    }



    public struct BehaviourMoveModifyer: IBehaviourModifier
    {
        public float SpeedModifier {get; private set; }
        public float RoamDistanceModifier {get; private set; }

        public BehaviourMoveModifyer(float speedModifier, float roamDistancModifier)
        {
            SpeedModifier = speedModifier;
            RoamDistanceModifier = roamDistancModifier;
        }
    }


    // EAT //
    public class BehaviourEatPlayer : BehaviourModel, IEatBehaviour
    {
        
        private float m_EnergyCost = 2; 
        private IEnumerable m_FoodReceived;

        public BehaviourEatPlayer(IEnumerable food)
        {
            m_FoodReceived = food;
        }

        public event Action<float> EnergyReceived;
        public event Action<float> EnergyWasted;
        public event Action<IEntity> FoodWasConsumed;
        

        public override void Do()
        {
            foreach (var food in m_FoodReceived)
            {
                if(food is IFood)
                {
                    Eat((IFood)food);
                    break;
                }
                    
            }
        }

        private void Eat(IFood food)
        {
            EnergyReceived?.Invoke(food.Energy);
            EnergyWasted?.Invoke(m_EnergyCost);
            FoodWasConsumed?.Invoke(food);
            Debug.Log($"{food.GetName()} was consumed");
        }

    }

    public class BehaviourEatAI : BehaviourModel, IEatBehaviour
    {
        
        private float m_EnergyCost = 2; 
        private IEnumerable m_FoodReceived;

        public BehaviourEatAI(IEnumerable food)
        {
            m_FoodReceived = food;
        }
        
        
        public event Action<float> EnergyWasted;
        public event Action<float> EnergyReceived;
        public event Action<IEntity> FoodWasConsumed;

        public override void Do()
        {
            foreach (var food in m_FoodReceived)
            {
                if(food is IFood)
                {
                    Eat((IFood)food);
                    break;
                }
                    
            }
        }

        private void Eat(IFood food)
        {
            EnergyReceived?.Invoke(food.Energy);
            EnergyWasted?.Invoke(m_EnergyCost);
            FoodWasConsumed?.Invoke(food);
            Debug.Log($"{food.GetName()} was consumed");
        }
    }


    // CHASE //
    public class BehaviourChaseAI : BehaviourModel, IBehaviour
    {
        public event Action<float> EnergyWasted;
        
        public override void Do()
        {


        }

    }


    // ATTACK //
    public class BehaviourAttackPlayer : BehaviourModel, IAttackBehaviour
    {
        
        private IEnumerable<IEntity> m_Entities;

        private Vector3 m_Position;
        private float m_EnergyCost = 10;
        private float m_Damage = 25;
        
        
        private Func<Vector3> GetPositionFunc;
        
        public event Action<float> EnergyWasted;
        public event Action<float, IEntity> EntityAttacked;



        public BehaviourAttackPlayer(IEnumerable<IEntity> entities, Func<Vector3> getPositionFunc)
        {
            m_Entities = entities;
            
            GetPositionFunc = getPositionFunc;
        }

        public override void Do()
        {
            if(FindClosest(out var entity))
                Attack((IEnemy)entity);
            
        }

        private void Attack(IEnemy enemy)
        {
            EnergyWasted?.Invoke(m_EnergyCost);
            enemy.Damage(m_Damage);
            EnergyWasted?.Invoke(m_EnergyCost);
            EntityAttacked?.Invoke(m_Damage, enemy);

        }

        private bool FindClosest(out IEntity targetEntity)
        {
            targetEntity = null;

            if((from IEntity entity in m_Entities select entity).Count() > 0)
                targetEntity = (from IEntity entity in m_Entities select entity).First();
            else
                return false;
 
                
            Vector3 currentPosition = GetPositionFunc();
            float minDistance = 1000;
            float distance = 0;
            

            foreach (var entity in m_Entities)
            {
                if(entity is IEnemy)
                {
                    distance = Vector3.Distance(currentPosition, entity.Position);
                    if(distance < minDistance)
                    {
                        minDistance = distance; 
                        targetEntity = entity;
                    }
                }
            }

            return true;
        }
    }
    
    public class BehaviourAttackAI : BehaviourModel, IAttackBehaviour
    {
        public event Action<float> EnergyWasted;
        public event Action<float, IEntity> EntityAttacked;

        private IEnumerable m_Entities;

        private float m_EnergyCost = 10;
        private float m_Damage = 25;

        public BehaviourAttackAI(IEnumerable entities)
        {
            m_Entities = entities;
        }

        public override void Do()
        {
            foreach (var entity in m_Entities)
            {
                if(entity is IPlayer)
                    Attack((IPlayer)entity);
                    break;
            }
        }

        private void Attack(IPlayer player)
        {
            EnergyWasted?.Invoke(m_EnergyCost);
            player.Damage(m_Damage);
        }

    }


    // MODEL //
    public abstract class BehaviourModel
    {
        
        public bool Modified {get; protected set;}
        
        public abstract void Do();
        
        public virtual void Modify(IBehaviourModifier modifier) { }
        public virtual void Default() { }


    }


    public interface IAttackBehaviour: IBehaviour
    {
        event Action<float, IEntity> EntityAttacked; 
    }

    public interface IEatBehaviour: IBehaviour
    {
        event Action<float> EnergyReceived;
        event Action<IEntity> FoodWasConsumed;
    }

    public interface IMoveBehaviour: IBehaviour
    {

    }

    public interface IChaseBehaviour: IBehaviour
    {

    }

    public interface IDieBehaviour: IBehaviour
    {

    }

    public interface IDamageBehaviour: IBehaviour
    {

    }



}

namespace APP
{
    public interface IBehaviour
    {
        bool Modified {get; }
        
        event Action<float> EnergyWasted;
        
        void Do();

        void Modify(IBehaviourModifier modifier);
        void Default();

    }

    public interface IBehaviourModifier
    {

    }

}