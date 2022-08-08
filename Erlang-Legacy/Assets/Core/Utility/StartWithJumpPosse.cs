using UnityEngine;
using Core.Player.Controller;

namespace Core.Utility
{
    public class StartWithJumpPosse : MonoBehaviour
    {
        // Player starts scene with jump posse
        void Start()
        {
            PlayerController.Instance.Animator.SetBool("jumping", true);
            PlayerController.Instance.Animator.SetTrigger("jump");
        }

    }
}
