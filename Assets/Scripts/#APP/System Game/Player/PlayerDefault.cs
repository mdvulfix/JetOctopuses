using System;
using UnityEngine;
using URandom = UnityEngine.Random;

using APP.Behaviour;

namespace APP.Player
{
    public class PlayerDefault : PlayerModel<PlayerDefault>, IPlayer
    {
        [SerializeField] private int m_Health;

    }



    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PlayerModel<TPlayer> : MonoBehaviour
    {
        public int Health { get; private set; }
        public int Energy { get; private set; }
        public int Score { get; private set; }

        public Rigidbody2D Rigidbody { get; private set; }

        private IBehaviour m_Move;
        private IBehaviour m_Eat;

        public void Move() =>
            m_Move.Do();

        public void Eat() =>
            m_Eat.Do();

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            m_Move = new BehaviourMoveDefault(Rigidbody, 2);
            m_Eat = new BehaviourEatDefault();
        }

        private void FixedUpdate()
        {
            Move();
            Eat();
        }

    }

}

namespace APP.Behaviour
{

    public class BehaviourModel<TBehaviour>
    {

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

        public void Do()
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
        public BehaviourMoveDefaultAI(Rigidbody2D rigidbody, int speed)
        {
            Speed = speed;
            Rigidbody = rigidbody;
        }

        public int Speed { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }

        private float m_CooldownTime = 1f;
        private bool m_ResetDirection;

        public void Do()
        {
            if (Cooldown() == false)
                return;

            Rigidbody.velocity = CalculateDirection();

        }

        private Vector3 CalculateDirection()
        {
            var moveHorizontal = URandom.Range(-1, 2);
            var moveVertical = URandom.Range(-1, 2);

            return new Vector3(moveHorizontal, moveVertical, 0).normalized * Speed;
        }

        private bool Cooldown()
        {
            m_CooldownTime -= Time.deltaTime;

            if (m_CooldownTime <= 0)
            {
                m_CooldownTime = URandom.Range(0, 6);
                return true;
            }

            return false;
        }

    }

    public class BehaviourEatDefault : BehaviourModel<BehaviourEatDefault>, IBehaviour
    {
        public void Do()
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

}

namespace APP
{
    public interface IPlayer
    {

    }
    
    public interface IBehaviour
    {
        void Do();
    }

}