using UnityEngine;
using System;


namespace Core.Combat
{
    public class Triggerable : MonoBehaviour
    {

        private bool enabledCollider = true;
        private Collider2D BodyCollider => GetComponent<Collider2D>();
        public Action<Collider2D> OnEnter { get; set; }

        public bool Enabled
        {
            get => enabledCollider; 
            set => HandleEnabled(value);
        }

        public void Awake()
        {
            if (BodyCollider == null)
            {
                Debug.LogError("May never trigger because not collider found in this object");
            }
            HandleEnabled(Enabled);
        }

        private void HandleEnabled(bool active)
        {
            enabledCollider = active;
            BodyCollider.enabled = enabledCollider;
        }

        // pre: --
        // post: search enemy controller in parent because of mechanism to 
        //        pass throught enemies without colliding
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!Enabled)
                return;

            OnEnter?.Invoke(other);
        }
    }
}
