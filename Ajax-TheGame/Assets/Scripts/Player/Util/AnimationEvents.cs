using Core.Player.Controller;
using UnityEngine;

namespace Core.Player.Util
{
    public class AnimationEvents : MonoBehaviour
    {
        private PlayerController Player => PlayerController.Instance;

        // pre: called at end of dash animation
        public void OnDashEnd1()
        {
            OnDashEnd();
        }

        private void OnDashEnd()
        {
            Player.OnDashCompletes();
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

    }

}
