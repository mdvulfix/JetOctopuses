using System;
using UnityEngine;

namespace APP.Player
{
    public class PlayerDefault: PlayerModel<PlayerDefault>
    {
        [SerializeField] private float m_Speed = 1f;
        //[SerializeField] private Vector3 m_Vector = Vector3.zero;
        
        [SerializeField] private Vector3 m_MoveDirection;

        public override void Move() 
        {
            Rigidbody.velocity = m_MoveDirection * m_Speed;
        }
        
        
        
        
        
        public override void Eat() { }

        private void Update() 
        {
            
            var moveHorizontal = 0;
            var moveVertical = 0;
            
            if(Input.GetKey(KeyCode.W))
                moveVertical = 1;
            
            if(Input.GetKey(KeyCode.S))
                moveVertical = -1;
            
            if(Input.GetKey(KeyCode.A))
                moveHorizontal = -1;
            
            if(Input.GetKey(KeyCode.D))
                moveHorizontal = 1;
            
            
            m_MoveDirection = new Vector3(moveHorizontal, moveVertical, 0).normalized;
            
            

        }

        private void FixedUpdate() 
        {
            Move();
            
        }


    
    
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PlayerModel<TPlayer>: MonoBehaviour
    {
        public int Health {get; private set; }
        public int Energy {get; private set; }
        public int Score {get; private set; }
        
        
        public Rigidbody2D Rigidbody {get; private set; }
        
        
        public abstract void Move(); 
        public abstract void Eat();


        private void Start() 
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    
    
    }




}

