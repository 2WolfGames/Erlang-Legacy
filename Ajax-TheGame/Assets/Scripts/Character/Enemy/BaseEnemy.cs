using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Character.Enemy
{
    public class BaseEnemy : BaseCharacter
    {
        public override void Hit(int damage, GameObject other = null, float recoverTime = 0)
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
