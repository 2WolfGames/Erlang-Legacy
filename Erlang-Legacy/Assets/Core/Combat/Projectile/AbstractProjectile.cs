using UnityEngine;


namespace Core.Combat.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public Vector2 force;

        public virtual void SetForce(Vector2 force)
        {
            this.force = force;
        }
    }
}
