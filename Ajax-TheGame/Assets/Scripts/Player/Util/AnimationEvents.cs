using Core.Player.Controller;
using UnityEngine;

namespace Core.Player.Util
{
    public class AnimationEvents : MonoBehaviour
    {
        private PlayerController Player => PlayerController.Instance;

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

        public void OnRay()
        {
            Player.InvokeRay();
        }

        public void OnRayStart(){
            Player.OnRayStarts();
        }

        public void OnRayEnd(){
            Player.OnRayEnd();
        }

    }

}
