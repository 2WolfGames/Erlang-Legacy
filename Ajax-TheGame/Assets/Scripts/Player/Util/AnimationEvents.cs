using Core.Player.Controller;
using UnityEngine;

namespace Core.Player.Util
{
    public class AnimationEvents : MonoBehaviour
    {
        private PlayerController Player => PlayerController.Instance;

        // desc: start of hit animation
        public void FreezeAjax()
        {
            Player.Controllable = false;
        }

        // desc: end of hit animation
        public void UnfreezeAjax()
        {
            Player.Controllable = true;
        }

        // pre: called at end of dash animation
        public void OnDashEnd()
        {
            Player.OnDashCompletes();
        }

        // pre: On recover start
        public void OnRecoverComplete()
        {
            Player.OnRecoverComplete();
        }
    }

}
