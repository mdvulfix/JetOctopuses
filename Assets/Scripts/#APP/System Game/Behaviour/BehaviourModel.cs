using UnityEngine;
using URandom = UnityEngine.Random;

namespace APP.Game
{
    public abstract class BehaviourModel<TBehaviour>
    {
        public abstract void Do();
    }


    public class BehaviourMoveDefault : BehaviourModel<BehaviourMoveDefault>, IBehaviour
    {
        public BehaviourMoveDefault() { }
        public BehaviourMoveDefault(Rigidbody2D rigidbody, int speed)
        {
            Speed = speed;
            Rigidbody = rigidbody;
        }

        public int Speed { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }

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

    public class BehaviourMoveDefaultAI : BehaviourModel<BehaviourMoveDefaultAI>, IBehaviour
    {
        public BehaviourMoveDefaultAI() { }
        public BehaviourMoveDefaultAI(
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

        private Rigidbody2D m_Rigidbody;
        private int m_Speed;
        private Vector3 m_RoamStartPosition;
        private float m_RoamDistance;

        private float m_CooldownTime = 1f;
        private float m_CooldownTimeMax = 5f;

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
 
    public class BehaviourEatDefault : BehaviourModel<BehaviourEatDefault>, IBehaviour
    {
        public override void Do()
        {

        }
    }
    
    public class BehaviourEatDefaultAI : BehaviourModel<BehaviourEatDefaultAI>, IBehaviour
    {
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

    public class BehaviourChaseDefaultAI : BehaviourModel<BehaviourChaseDefaultAI>, IBehaviour
    {
        public override void Do()
        {


        }

    }

    public class BehaviourAttackDefaultAI : BehaviourModel<BehaviourAttackDefaultAI>, IBehaviour
    {
        public override void Do()
        {


        }

    }



}

namespace APP
{
    public interface IBehaviour
    {
        void Do();
    }

}