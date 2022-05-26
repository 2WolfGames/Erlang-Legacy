using Core.Shared.Enum;
using UnityEngine;

namespace Core.Player.Controller
{
    public class FacingController : MonoBehaviour
    {
        public PlayerFacing Facing { get; private set; }

        public int FacingToInt { get => (int)Facing * 2 - 1; }

        public void Update()
        {
            if (!PlayerController.Instance.Controllable)
                return;
                
            var input = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(input) <= 0)
                return;

            if (input < 0)
                Facing = PlayerFacing.Left;
            else Facing = PlayerFacing.Right;
        }

        //pre: --
        //post: sets playerFacing
        public void SetFacing(PlayerFacing playerFacing)
        {
            Facing = playerFacing;
        }

    }
}
