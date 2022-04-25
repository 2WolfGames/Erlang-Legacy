using UnityEngine;

namespace Core.Combat.Projectile
{
    // description:
    //  Ajax ray attack
    public class RayProjectile : AbstractProjectile
    {
        Rigidbody2D body;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
        }

        void FixedUpdate()
        {
            body.velocity = force;
        }
    }

}
