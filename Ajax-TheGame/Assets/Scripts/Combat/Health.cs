using UnityEngine;

namespace Core.Combat
{
    public class Health : MonoBehaviour
    {
        [Range(1, 10000)][SerializeField] int lifes = 100;

        public int Life { get => lifes; set => lifes = value; }

        /**
            this fn returns true when
            the state of lifes controller is less or equal zero

            at that moment represents who ever 
            use this class is dead
        */
        public bool TakeLife(int amount)
        {
            this.lifes -= amount;
            return this.lifes <= 0;
        }

        public void AddLife(int amount)
        {
            this.lifes += amount;
        }

        public void SetLifes(int lifes)
        {
            this.lifes = lifes;
        }
    }
}
