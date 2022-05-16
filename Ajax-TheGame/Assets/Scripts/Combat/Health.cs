using UnityEngine;

namespace Core.Combat
{
    public class Health : MonoBehaviour
    {
        [Range(1, 10000)][SerializeField] int hp = 100;
        private int startingHp;
        public int HP { get => hp; set => hp = value; }

        public void Awake()
        {
            this.startingHp = hp;
        }

        public bool TakeHP(int amount)
        {
            this.hp -= amount;
            return this.hp <= 0;
        }

        public void AddHP(int amount)
        {
            this.hp += amount;
        }

        public void SetHP(int hp)
        {
            this.hp = hp;
            this.startingHp = this.hp;
        }

        public void Revive()
        {
            this.hp = this.startingHp;
        }
    }
}
