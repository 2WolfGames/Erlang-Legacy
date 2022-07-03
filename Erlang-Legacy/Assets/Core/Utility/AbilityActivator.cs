using System.Collections;
using Core.Player.Controller;
using Core.ScriptableEffect;
using UnityEngine;

namespace Core.Utility
{
    public class AbilityActivator : MonoBehaviour
    {
        public AbilityActivatorEffect effect;

        private bool activated;
        private PlayerController player => PlayerController.Instance;
        private Animator animator => GetComponent<Animator>();

        public void Disposable()
        {
            gameObject.Disposable(0f);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (CanApplyEffect(other))
            {
                effect.Apply(other.gameObject);
                activated = true;
                animator?.SetTrigger("Activated");
            }
        }

        private bool CanApplyEffect(Collider2D other)
        {
            return other.CompareTag("Player") && !activated;
        }
    }
}
