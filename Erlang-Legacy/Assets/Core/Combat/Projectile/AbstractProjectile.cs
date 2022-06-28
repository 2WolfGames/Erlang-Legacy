using Core.Manager;
using UnityEngine;


namespace Core.Combat.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public Vector2 force;
        public ParticleSystem explosionEffect;
        public AudioSource explosionSound;
        public Collider2DEvent OnEnter, OnExit;

        public virtual void SetForce(Vector2 force)
        {
            this.force = force;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (explosionEffect != null)
                EffectManager.Instance?.PlayOneShot(explosionEffect, transform.position);
            if (explosionSound != null)
                explosionSound.Play();
            if (OnEnter != null)
                OnEnter.Invoke(other);
        }
    }
}
