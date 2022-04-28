using System;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class StatsData
    {
        [Header("Movement")]
        [SerializeField] float movementSpeed; // basic player speed movement
        [SerializeField] float dashSpeed; // player speed at dashing
        [SerializeField] float jumpPower;
        [SerializeField] [Range(0.1f, 1f)] float airDrag;


        [Header("Damage")]
        [SerializeField] float dashDamage;
        [SerializeField] float rayDamage; // how much damage ray does
        [SerializeField] float punchDamage;


        [Header("Cooldown or events duration")]
        [SerializeField] float dashCooldown; // how much it takes to trigger dash ability again
        [SerializeField] float rayCooldown; // how much it takes to through a ray again
        [SerializeField] float holdingAfterJump; // how much time can you press jump key to continue jumping
        [SerializeField] float recoverTimeoutAfterHit; // how much time does it takes to player to recover after hit animations

        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
        public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
        public float JumpPower { get => jumpPower; set => jumpPower = value; }
        public float AirDrag { get => airDrag; set => airDrag = value; }
        public float DashDamage { get => dashDamage; set => dashDamage = value; }
        public float RayDamage { get => rayDamage; set => rayDamage = value; }
        public float PunchDamage { get => punchDamage; set => punchDamage = value; }
        public float RayCooldown { get => rayCooldown; set => rayCooldown = value; }
        public float DashCooldown { get => dashCooldown; set => dashCooldown = value; }
        public float HoldingAfterJump { get => holdingAfterJump; set => holdingAfterJump = value; }
        public float RecoverTimeoutAfterHit { get => recoverTimeoutAfterHit; set => recoverTimeoutAfterHit = value; }
    }
}

