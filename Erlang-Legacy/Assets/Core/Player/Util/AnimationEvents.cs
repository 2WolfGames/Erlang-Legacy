using Core.Player.Controller;
using UnityEngine;

namespace Core.Player.Util
{
    public class AnimationEvents : MonoBehaviour
    {
        private PlayerController player => PlayerController.Instance;

        // pre: called at end of dash animation
        public void OnDashEnd1()
        {
            OnDashEnd();
        }

        private void OnDashEnd()
        {
            player.OnDashComplete();
        }

        public void OnHitEnd()
        {
            player.OnRecoverComplete();
        }

        public void OnRay()
        {
            // triggers ray power ball
            var abilityController = player.GetComponent<AbilityController>();
            abilityController.InvokeRayBallInstance();
        }

        public void OnRayEnd()
        {
            var abilityController = player.GetComponent<AbilityController>();
            abilityController.RayAbilityComplete();
        }

        public void OnPunch()
        {
        }

        public void OnPunchStart()
        {
        }

        public void OnPunchEnd()
        {
            var abilityController = player.GetComponent<AbilityController>();
            abilityController.PunchEnd();
        }

    }

}
