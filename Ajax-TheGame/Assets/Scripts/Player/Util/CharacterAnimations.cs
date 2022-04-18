using UnityEngine;


namespace Core.Player.Util
{
    public class CharacterAnimations
    {
        public static readonly int Die = Animator.StringToHash("die");
        public static readonly int Punch = Animator.StringToHash("punch");
        public static readonly int Dash = Animator.StringToHash("dash");
        public static readonly int StartJump = Animator.StringToHash("jump");
        public static readonly int Jumping = Animator.StringToHash("jumping");
        public static readonly int Run = Animator.StringToHash("running");
        public static readonly int Recover = Animator.StringToHash("blink");
        public static readonly int BackHurt = Animator.StringToHash("twist");
        public static readonly int FrontHurt = Animator.StringToHash("hit");
        public static readonly string DashAnimationName = "dash";
    }
}
