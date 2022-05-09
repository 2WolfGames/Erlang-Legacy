using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat
{
    public class Triggerable : MonoBehaviour
    {

        private Collider2D BodyCollider => GetComponent<Collider2D>();
        public Action<Collider2D> OnEnter { get; set; }
        public UnityEvent OnExit;

        public bool Enabled
        {
            get => BodyCollider.enabled;
            set => BodyCollider.enabled = value;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!Enabled)
                return;

            OnEnter?.Invoke(other);
        }
    }
}
