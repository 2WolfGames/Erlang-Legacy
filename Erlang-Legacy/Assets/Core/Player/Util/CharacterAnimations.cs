using UnityEngine;


namespace Core.Player.Util
{
    public class CharacterAnimations
    {
        public static readonly int Die = Animator.StringToHash("die");
        public static readonly int Ray = Animator.StringToHash("ray");
        public static readonly int LPunch = Animator.StringToHash("lpunch");
        public static readonly int RPunch = Animator.StringToHash("rpunch");
        public static readonly int Dash = Animator.StringToHash("dash");
        public static readonly int StartJump = Animator.StringToHash("jump");
        public static readonly int Jumping = Animator.StringToHash("jumping");
        public static readonly int Running = Animator.StringToHash("running");
        public static readonly int Recover = Animator.StringToHash("blink");
        public static readonly int BackHurt = Animator.StringToHash("hit_backward");
        public static readonly int FrontHurt = Animator.StringToHash("hit_forward");
        public static readonly int Blink = Animator.StringToHash("blink");
        public static readonly string DashAnimationName = "dash";
    }
}
