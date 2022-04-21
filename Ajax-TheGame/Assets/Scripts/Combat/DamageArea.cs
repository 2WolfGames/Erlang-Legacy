using UnityEngine;
using System;


namespace Core.Combat
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField][Range(10, 1000)] int damage = 100;
        public int Damage { get => damage; set => damage = value; }
        public Action<Collider2D> OnHit { get; set; }
        private Collider2D BodyCollider => GetComponent<Collider2D>();


        // pre: --
        // post: active or disable bodycollider
        public void SetEnabled(bool value)
        {
            BodyCollider.enabled = value;
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
