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
            player.InvokeRay();
        }

        public void OnRayStart()
        {
            player.OnRayStarts();
        }

        public void OnRayEnd()
        {
            player.OnRayEnd();
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
