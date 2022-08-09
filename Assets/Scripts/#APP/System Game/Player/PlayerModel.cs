using System;
using UnityEngine;

namespace APP.Game
{
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


}

namespace APP
{
    public interface IPlayer
    {

    }
}