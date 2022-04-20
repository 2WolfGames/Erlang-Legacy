using Core.Player.Controller;
using UnityEngine;


// todo: change name for player bla
namespace Core.Player.Util
{
    public class AnimationControllerEvents : MonoBehaviour
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
            Player.GetComponent<MovementController>().EndDash();
        }

        // pre: On recover start
        public void OnRecoverComplete()
        {
            Debug.Log("hola");
            Player.OnRecoverComplete();
        }

        
        // pre: On recover start
        public void OnRecoverStart()
        {
            Debug.Log("hola");
            Player.OnRecoverStart();
        }
    }

}
