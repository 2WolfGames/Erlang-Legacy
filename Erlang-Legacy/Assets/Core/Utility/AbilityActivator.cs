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


        public void OnTriggerEnter2D(Collider2D other)
        {
            if (CanApplyEffect(other))
            {
                effect.Apply(other.gameObject);
                activated = true;
            }
        }

        private bool CanApplyEffect(Collider2D other)
        {
            return other.CompareTag("Player") && !activated;
        }
    }
}
