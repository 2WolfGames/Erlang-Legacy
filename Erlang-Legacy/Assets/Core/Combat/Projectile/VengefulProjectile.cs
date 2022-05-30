using UnityEngine;

namespace Core.Combat.Projectile
{
    public class VengefulProjectile : AbstractProjectile
    {
        private Rigidbody2D body;

        public void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
        }

        public void Update()
        {
            body.velocity = force;
        }
    }
}
