using UnityEngine;
using URandom = UnityEngine.Random;

namespace App.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Transform))]
    public abstract class FoodModel<TFood> : MonoBehaviour
    {
        private Rigidbody2D m_Rigidbody;
        private Collider2D m_Collider;
        private Transform m_Transform;

        public float Energy { get; private set; }
        public Vector3 Position => transform.position;

        public virtual void Configure(params object[] args)
        {

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Collider = GetComponent<Collider2D>();
            m_Transform = GetComponent<Transform>();

            Energy = 50f;

        }

        public virtual void Init()
        {
            var direction = new Vector3(URandom.Range(-5, 6) * 0.1f, URandom.Range(-5, 6) * 0.1f, 0);
            AddForce(direction);

        }

        public virtual void Dispose()
        {

        }






        public void SetPosition(Vector3 position) =>
            m_Transform.position = position;

        public void AddForce(Vector3 direction) =>
            m_Rigidbody.AddForce(direction, ForceMode2D.Impulse);

        private void Start()
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
    public interface IFood : IEntity
    {
        float Energy { get; }
    }

}