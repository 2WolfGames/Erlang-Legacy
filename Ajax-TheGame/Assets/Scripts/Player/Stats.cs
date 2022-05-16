using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(menuName = "Player/Stats")]
    public class Stats : ScriptableObject
    {
        [Header("Movement")]
        public float movementSpeed; // basic player speed movement
        public float dashSpeed; // player speed at dashing
        public float jumpPower;
        [Range(0.1f, 1f)] public float airDrag;


        [Header("Damage")]
        public float dashDamage;
        public float rayDamage; // how much damage ray does
        public float punchDamage;


        [Header("Cooldown or events duration")]
        public float dashCooldown; // how much it takes to trigger dash ability again
        public float rayCooldown; // how much it takes to through a ray again
        public float holdingAfterJump; // how much time can you press jump key to continue jumping
        public float recoverTimeoutAfterHit; // how much time does it takes to player to recover after hit animations
    }
}
