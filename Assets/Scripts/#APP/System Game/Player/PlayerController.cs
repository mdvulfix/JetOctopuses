using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APP.Game
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private PlayerDefault m_Player;

        public event Action<IEntity> FoodWasConsumed;
        public event Action<IEntity> EntityAttacked;
        
        
        public virtual void Configure(params object[] args)
        {

        }

        
        public virtual void Init()
        {
            m_Player.FoodWasConsumed += OnFoodWasConsumed;
            m_Player.EntityAttacked += OnEntityAttacked;

        }

        public virtual void Dispose()
        {
            m_Player.FoodWasConsumed -= OnFoodWasConsumed;
            m_Player.EntityAttacked -= OnEntityAttacked;
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
            if(Input.GetMouseButtonUp(1))
                m_Player.Eat();
        
            if(Input.GetMouseButtonUp(0))
                m_Player.Attack();
        
        }

        private void FixedUpdate()
        {
            m_Player.Move();
        }


        private void OnFoodWasConsumed(IEntity food)
        {
            FoodWasConsumed?.Invoke(food);
        }

        private void OnEntityAttacked(IEntity enemy)
        {
            EntityAttacked?.Invoke(enemy);
        }

    }
}