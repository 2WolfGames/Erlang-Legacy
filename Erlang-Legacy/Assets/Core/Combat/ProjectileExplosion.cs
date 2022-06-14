using Core.ScriptableEffect;
using Core.Utility;
using UnityEngine;

namespace Core.Combat
{
    [RequireComponent(typeof(Disposable))]
    public class ProjectileExplosion : MonoBehaviour
    {
        public HealthTaker healthTaker;
        public ParticleSystem explosionEffectPrefab;

        public void Start()
        {
            if (explosionEffectPrefab)
            {
                var explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                explosion.Play();
                explosion.gameObject.Disposable(10f);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                this.healthTaker.Apply(gameObject, other.gameObject);
            }
        }
    }
}

