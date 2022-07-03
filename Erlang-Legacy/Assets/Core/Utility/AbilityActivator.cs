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
                // StartCoroutine(AfterAdquiredAbility());
            }
        }

        private IEnumerator AfterAdquiredAbility()
        {
            // BigNotificationManager.Instance.ShowNotification(
            //     effect.ability.image,
            //     effect.ability.title,
            //     effect.ability.description
            // );
            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(0.1f);
            Time.timeScale = 1f;
        }

        private bool CanApplyEffect(Collider2D other)
        {
            return other.CompareTag("Player") && !activated;
        }
    }
}
