using UnityEngine;

using APP.Behaviour;

namespace APP.Enemy
{
    public class EnemyDefault : EnemyModel<EnemyDefault>, IEnemy
    {
        [SerializeField] private int m_Health;
        [SerializeField] private int m_Speed;
        [SerializeField] private Vector3 m_Direction;

        public void Init ()
        {

        }

    }

    [RequireComponent (typeof (Rigidbody2D))]
    public abstract class EnemyModel<TEnemy> : MonoBehaviour
    {
        
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Energy { get; private set; }

        public Rigidbody2D Rigidbody { get; private set; }


        public virtual void Attack () { }
        public virtual void Damage () { }
        public virtual void Die () { }

        private IBehaviour m_Move;
        private IBehaviour m_Eat;

        public void Move() =>
            m_Move.Do();
        

        public void Eat() =>
            m_Eat.Do();

        private void Awake()
        {
            Name = gameObject.name;

            Rigidbody = GetComponent<Rigidbody2D>();
            m_Move = new BehaviourMoveDefaultAI(Rigidbody, 2);
            m_Eat = new BehaviourEatDefault();
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
    public interface IEnemy
    {
        
        string Name {get; }

        void Move ();
        void Eat ();
        void Attack ();
        void Damage ();
        void Die ();
    }
}