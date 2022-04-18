using Core.Player.Controller;
using UnityEngine;

namespace Core.Player.Util
{
    public class AnimationControllerEvents : MonoBehaviour
    {
        // desc: start of hit animation
        public void FreezeAjax()
        {
            var player = PlayerController.Instance;
            player.Controllable = false;
        }

        // desc: end of hit animation
        public void UnfreezeAjax()
        {
            var player = PlayerController.Instance;
            player.Controllable = true;
        }

        // pre: called at end of dash animation
        public void OnDashEnd()
        {
            var player = PlayerController.Instance;
            player.GetComponent<MovementController>().EndDash();
        }
    }

}
