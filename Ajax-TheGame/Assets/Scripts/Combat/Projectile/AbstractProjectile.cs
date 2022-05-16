using System;
using Core.Utility;
using UnityEngine;

// TODO: change action event to unity actions event

namespace Core.Combat.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public ParticleSystem explosionEffect;
        public AudioClip splatterSound;
        public bool destroyAfterCollision = false;

        public GameObject Shooter { get; set; }

        protected Vector2 force;

        public event Action<AbstractProjectile> OnProjectileDestroyed;

        public event Action<Collider2D> OnColliding;

        public virtual void SetForce(Vector2 force)
        {
            this.force = force;
        }

        protected void DestroyProjectile()
        {
            OnProjectileDestroyed?.Invoke(this);

            if (splatterSound != null)
                SoundManager.Instance?.PlaySoundAtLocation(splatterSound, transform.position, 0.75f);

            EffectManager.Instance?.PlayOneShot(explosionEffect, transform.position);

            if (destroyAfterCollision)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Can't shoot yourself
            if (collision.gameObject == Shooter)
                return;

            OnColliding?.Invoke(collision);

            DestroyProjectile();
        }
    }
}
