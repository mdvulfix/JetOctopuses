
using UnityEngine;

using Core;
using Core.Spawn;

namespace Test.Spawn
{
    public class TestSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_Obj;
        [SerializeField] private SpawnerDefault m_Spawner;

        float m_Timer = 1f;

        private void OnEnable()
        {
            m_Spawner = SpawnerDefault.Get(new SpawnerConfig());

        }

        private void OnDisable()
        {
            m_Spawner.Dispose();
        }

        private void Update()
        {
            if (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
                Debug.Log(m_Timer);
                return;
            }


            var position = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            m_Spawner.Spawn(m_Obj, position);
            m_Timer = 1f;

        }



    }

}

