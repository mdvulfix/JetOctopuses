using UnityEngine;

namespace APP.Game
{
    public abstract class FoodModel<TFood> : MonoBehaviour
    {
        public float Energy { get; private set; }

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