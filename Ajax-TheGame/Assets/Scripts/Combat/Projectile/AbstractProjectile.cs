using System;
using Core.Util;
using UnityEngine;

namespace Core.Combat.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public ParticleSystem explosionEffect;
        public AudioClip splatterSound;

        public GameObject Shooter { get; set; }
        public bool DestroyOnColliding { get; set; } = false;

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

            if (DestroyOnColliding)
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