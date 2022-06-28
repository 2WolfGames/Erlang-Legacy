using UnityEngine;

namespace Core.Combat.Projectile
{
    public class VengefulProjectile : AbstractProjectile
    {
        public void Awake()
        {
            body.bodyType = RigidbodyType2D.Kinematic;
        }

        public void Update()
        {
            body.velocity = force;
        }
    }
}
