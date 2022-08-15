using System;
using System.Collections;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace APP.Game
{
    public abstract class BehaviourModel<TBehaviour>
    {
        public abstract void Do();
    }


    public class BehaviourMovePlayer : BehaviourModel<BehaviourMovePlayer>, IBehaviour
    {
        public BehaviourMovePlayer() { }
        public BehaviourMovePlayer(Rigidbody2D rigidbody, int speed)
        {
            Speed = speed;
            Rigidbody = rigidbody;
        }

        
        public int Speed { get; private set; }
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

    public class BehaviourMoveAI : BehaviourModel<BehaviourMoveAI>, IBehaviour
    {
        private Rigidbody2D m_Rigidbody;
        private int m_Speed;
        private Vector3 m_RoamStartPosition;
        private float m_RoamDistance;

        private float m_CooldownTime = 1f;
        private float m_CooldownTimeMax = 5f;
        
        public BehaviourMoveAI() { }
        public BehaviourMoveAI(
            Rigidbody2D rigidbody,
            int speed,
            Vector2 roamStartPosition,
            float roamDistance)
        {
            m_Speed = speed;
            m_RoamStartPosition = roamStartPosition;
            m_RoamDistance = roamDistance;
            m_Rigidbody = rigidbody;
        }


        public event Action<float> EnergyWasted;
        
        public override void Do()
        {
            if (Cooldown() == false)
                return;

            m_Rigidbody.velocity = GetDirection();

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
 
    public class BehaviourEatPlayer : BehaviourModel<BehaviourEatPlayer>, IBehaviour
    {
        private IEnumerable m_FoodRevealed;

        public BehaviourEatPlayer(IEnumerable food)
        {
            m_FoodRevealed = food;
        }

        public event Action<float> EnergyReceived;
        public event Action<float> EnergyWasted;

        public override void Do()
        {
            foreach (var food in m_FoodRevealed)
            {
                if(food is IFood)
                    Eat((IFood)food);
            }
        }

        private void Eat(IFood food)
        {
            EnergyReceived?.Invoke(food.Energy);
        }

    }
    
    public class BehaviourEatAI : BehaviourModel<BehaviourEatAI>, IBehaviour
    {
        
        public event Action<float> EnergyWasted;
        
        public override void Do()
        {
            if(FindFood())
                Catch();

        }

        public bool FindFood()
        {
            return false;
        }

        public void Catch()
        { 

        } 
    }

    public class BehaviourChaseAI : BehaviourModel<BehaviourChaseAI>, IBehaviour
    {
        public event Action<float> EnergyWasted;
        
        public override void Do()
        {


        }

    }

    public class BehaviourAttackPlayer : BehaviourModel<BehaviourAttackPlayer>, IAttackBehaviour
    {
        public event Action<float> EnergyWasted;
        public event Action<float, IEnemy> EnemyAttacked;

        private IEnumerable m_EnemyFound;

        private float m_EnergyCost = 10f;
        private float m_Damage = 25;

        public BehaviourAttackPlayer(IEnumerable enemy)
        {
            m_EnemyFound = enemy;
        }

        public override void Do()
        {
            foreach (var enemy in m_EnemyFound)
            {
                if(enemy is IEnemy)
                    Attack((IEnemy)enemy);
            }
        }

        private void Attack(IEnemy enemy)
        {
            EnergyWasted?.Invoke(m_EnergyCost);
            EnemyAttacked?.Invoke(m_Damage, enemy);
        }

    }
    
    public class BehaviourAttackAI : BehaviourModel<BehaviourAttackAI>, IBehaviour
    {
        public event Action<float> EnergyWasted;
        
        public override void Do()
        {


        }

    }



}

namespace APP
{
    public interface IBehaviour
    {
        event Action<float> EnergyWasted;
        void Do();
    }

    public interface IAttackBehaviour: IBehaviour
    {
        event Action<float, IEnemy> EnemyAttacked; 
    }

}