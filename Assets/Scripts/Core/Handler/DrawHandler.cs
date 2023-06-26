using System;
using UnityEngine;
namespace Core
{
    public class DrawHandler : MonoBehaviour
    {
        public float m_Radius = 1; //  Radius of ring
        public Color m_Color = Color.green; //  Wireframe color


        void OnDrawGizmos()
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
    }
}
