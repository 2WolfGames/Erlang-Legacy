using UnityEngine;

namespace Core.Combat.Projectile
{
    public class AcceleratingProjectile : AbstractProjectile
    {
        public float accelaration = 5.0f;
        private Vector3 direction = new Vector3(1, 0, 0);
        private float velocity = 0f;

        public override void SetForce(Vector2 force)
        {
            this.force = force;
            direction = force.normalized;
        }

        public void Update()
        {
            velocity += accelaration * Time.deltaTime;
            body.velocity = direction * velocity;
        }
    }
}
