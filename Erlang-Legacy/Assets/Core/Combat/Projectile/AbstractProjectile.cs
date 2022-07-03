using System.Collections.Generic;
using Core.Manager;
using Core.Player.Controller;
using Core.Utility;
using UnityEngine;


namespace Core.Combat.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public Vector2 force;
        public ParticleSystem explosionEffect;
        public AudioSource explosionSound;
        public Collider2DEvent OnEnter;
        public GameObject Shooter;
        public bool destroyOnCollision = false;
        protected Rigidbody2D body => GetComponent<Rigidbody2D>();

        public virtual void SetForce(Vector2 force)
        {
            this.force = force;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == Shooter) return;

            if (explosionEffect != null)
                EffectManager.Instance?.PlayOneShot(explosionEffect, transform.position);
            if (explosionSound != null)
                explosionSound.Play();
            if (OnEnter != null)
                OnEnter.Invoke(other);
            if (destroyOnCollision)
            {
                // if some enemy uses this class, don't destroy if collide with player when he is protected
                if (other.CompareTag("Player"))
                {
                    var player = other.gameObject.GetComponent<PlayerController>();
                    if (player.IsProtected) return;
                }
                gameObject.Disposable(0.01f);
            }
        }

    }
}
