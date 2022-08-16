using System;
using UnityEngine;

namespace APP.Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ControlZone : MonoBehaviour, IControlZone
    {

        [SerializeField] private float m_Radius = 1;
        [SerializeField] private Color m_Color = Color.green;

        public float Radius { get => m_Radius; set => m_Radius = value; }

        public event Action<IEntity> InZone;
        public event Action<IEntity> OutZone;

        private CircleCollider2D m_Collider;

        private void Awake()
        {
            m_Collider = GetComponent<CircleCollider2D>();
            m_Collider.radius = m_Radius;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<IEntity>(out var entity))
                InZone?.Invoke(entity);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent<IEntity>(out var entity))
                OutZone?.Invoke(entity);
        }

    #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            //Set matrix
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            //Set color
            Color defaultColor = Gizmos.color;
            Gizmos.color = m_Color;

            //Draw a ring
            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.01f)
            {
                float x = m_Radius * Mathf.Cos(theta);
                float y = m_Radius * Mathf.Sin(theta);

                Vector3 endPoint = new Vector3(x, y, 0);
                if (theta == 0)
                    firstPoint = endPoint;
                else
                    Gizmos.DrawLine(beginPoint, endPoint);

                beginPoint = endPoint;
            }

            //Draw the last segment
            Gizmos.DrawLine(firstPoint, beginPoint);
            //Restore default colors
            Gizmos.color = defaultColor;
            //Restore default matrix
            Gizmos.matrix = defaultMatrix;
        }

    #endif

    }

}

namespace APP
{
    public interface IControlZone
    {
        event Action<IEntity> InZone;
        event Action<IEntity> OutZone;
    }

}