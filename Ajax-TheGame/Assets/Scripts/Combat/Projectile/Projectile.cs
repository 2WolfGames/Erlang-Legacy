using UnityEngine;

namespace Core.Combat.Projectile
{
    public class Projectile : AbstractProjectile
    {
        public override void SetForce(Vector2 force)
        {
            this.force = force;
            GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }
}
