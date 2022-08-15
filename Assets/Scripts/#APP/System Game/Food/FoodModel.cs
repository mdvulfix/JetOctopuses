using UnityEngine;

namespace APP.Game
{
    public abstract class FoodModel<TFood> : MonoBehaviour
    {
        
        public float Energy { get; private set; }
        public Vector3 Position => transform.position;

        
        public void SetPosition(Vector3 position) => 
            transform.position = position;
        
        private void Start() 
        {
            Energy = 50f;


        }


    }

}

namespace APP
{
    public interface IFood: IEntity
    {
        float Energy { get; }
    }

}