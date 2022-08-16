using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APP.Game
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private PlayerDefault m_Player;

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Space))
                m_Player.Eat();
        }

        private void FixedUpdate()
        {
            m_Player.Move();
        }




    }
}