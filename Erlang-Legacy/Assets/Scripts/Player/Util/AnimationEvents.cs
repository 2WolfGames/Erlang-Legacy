using Core.Player.Controller;
using UnityEngine;

namespace Core.Player.Util
{
    public class AnimationEvents : MonoBehaviour
    {
        private PlayerController Player => PlayerController.Instance;
        private AbilityController playerAbilityController => Player.GetComponent<AbilityController>();

        // pre: called at end of dash animation
        public void OnDashEnd1()
        {
            OnDashEnd();
        }

        private void OnDashEnd()
        {
            Player.OnDashComplete();
        }

        public void OnHitEnd()
        {
            Player.OnRecoverComplete();
        }

        public void OnRay()
        {
            Player.InvokeRay();
        }

        public void OnRayStart()
        {
            Player.OnRayStarts();
        }

        public void OnRayEnd()
        {
            Player.OnRayEnd();
        }

        public void OnPunch()
        {
            playerAbilityController?.OnFlurryPunchingPunch();
        }

        public void OnPunchStart()
        {
            playerAbilityController?.OnFlurryPunchingStart();
        }

        public void OnPunchEnd()
        {
            playerAbilityController?.OnFlurryPunchingEnd();
        }

    }

}
