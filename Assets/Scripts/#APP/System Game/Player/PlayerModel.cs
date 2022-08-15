using System;
using System.Collections.Generic;
using UnityEngine;

namespace APP.Game
{
    public abstract class PlayerModel<TPlayer> : MonoBehaviour
    {
        public int Health { get; private set; }
        public int Energy { get; private set; }
        public int Score { get; private set; }

        
        [Header("Player components")]
        [SerializeField] private Rigidbody2D m_PlayerBody;
        [SerializeField] private ControlZone m_PlayerVision;
        [SerializeField] private ControlZone m_PlayerAttackZone;
        [SerializeField] private ControlZone m_PlayerEatZone;

        private IBehaviour m_MoveBehaviour;
        private IBehaviour m_EatBehaviour;
        private IBehaviour m_AttackBehaviour;

        private List<IEntity> m_EntiesInEatRange;
        private List<IEntity> m_EntiesInAttackRange;
        private List<IEntity> m_EntiesInVisionkRange;

        
        public void Move() =>
            m_MoveBehaviour.Do();

        public void Eat() =>
            m_EatBehaviour.Do();

        public void Attack() =>
            m_AttackBehaviour.Do();
     
        private void Awake()
        {

            m_MoveBehaviour = new BehaviourMovePlayer(m_PlayerBody, 2);

  
            m_EntiesInEatRange = new List<IEntity>();
            m_EatBehaviour = new BehaviourEatPlayer(m_EntiesInEatRange);

            m_EntiesInAttackRange = new List<IEntity>();
            m_EatBehaviour = new BehaviourAttackPlayer(m_EntiesInAttackRange);
        
        }

        private void FixedUpdate()
        {
            Move();
            Eat();
        }

    }

}


namespace APP
{
    public interface IPlayer: IEntity
    {

    }
}