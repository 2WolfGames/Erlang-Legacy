using System.Collections;
using Core.Character.Enemy;
using UnityEngine;
using System;


namespace Core.Combat
{
    public class HitArea : MonoBehaviour
    {
        [SerializeField][Range(10, 1000)] int hitDamage = 100;
        [SerializeField] float hitDuration;

        public int Damage
        {
            get => hitDamage;
            set { hitDamage = value; }
        }

        public float Duration
        {
            get => hitDuration;
            set { hitDuration = value; }
        }

        public Action<Collider2D> OnHit;

        public IEnumerator Hit(float time)
        {
            var hitArea = GetComponent<Collider2D>();
            hitArea.enabled = true;
            yield return new WaitForSeconds(time);
            hitArea.enabled = false;
        }

        public IEnumerator Hit()
        {
            var hitArea = GetComponent<Collider2D>();
            hitArea.enabled = true;
            yield return new WaitForSeconds(Duration);
            hitArea.enabled = false;
        }

        // pre: --
        // post: search enemy controller in parent because of mechanism to 
        //        pass throught enemies without colliding
        public void OnTriggerEnter2D(Collider2D other)
        {
            OnHit?.Invoke(other);
        }
    }
}
