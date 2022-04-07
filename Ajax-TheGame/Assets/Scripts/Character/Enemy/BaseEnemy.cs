using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// todo: transform this class
namespace Core.Character.Enemy
{
    public class BaseEnemy : BaseCharacter
    {
        public override void Hurt(int damage, GameObject other = null)
        {
            TakeLife(damage);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
