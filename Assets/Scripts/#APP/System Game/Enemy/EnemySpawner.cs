using UnityEngine;

namespace APP.Game
{
    public class EnemySpawner
    {
        private GameObject m_Root;
        private Sprite m_Sprite;

        private string m_Label;
        private float m_Scale;
        private Color m_Color;

        public EnemySpawner (Sprite sprite, GameObject root = null)
        {
            if (root != null)
                m_Root = root;

            m_Sprite = sprite;
            m_Label = "Enemy";
            m_Scale = 0.5f;
            m_Color = Color.blue;
        }

        public IEnemy Spawn (Vector3 position = default (Vector3))
        {
            var obj = new GameObject (m_Label);
            obj.transform.SetParent (m_Root.transform);
            obj.transform.position = position;
            obj.transform.localScale = new Vector3 (m_Scale, m_Scale, 0);

            var renderer = obj.AddComponent<SpriteRenderer> ();
            renderer.sprite = m_Sprite;
            renderer.color = m_Color;

            var collider = obj.AddComponent<CircleCollider2D>();
            var rigidbody = obj.AddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;

            return obj.AddComponent<EnemyDefault> ();
        }

    }

}