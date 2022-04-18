using UnityEngine;
using System;
using Core.Util.Serializable;
using System.Collections.Generic;


// TODO: rename movement controller to player movement manager
namespace Core.Character.Player
{
    public class PlayerMovementManager : MonoBehaviour
    {
        [SerializeField] Circle wallChecker;
        [SerializeField] Circle landChecker;
        [SerializeField] LayerMask whatIsGround;
        [SerializeField] List<LayerMask> whatEndsDash;

        private Circle WallChecker { get => wallChecker; set => wallChecker = value; }
        private Circle LandChecker { get => landChecker; set => landChecker = value; }
        private LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
        private float BaseGravityScale => Player.BaseGravityScale;
        private BasePlayer Player => BasePlayer.Instance;
        private float FacingValue => Player.FacingValue;
        private float AirDrag => 1f - Player.PlayerData.Stats.AirDrag;
        private Rigidbody2D Body => Player.Body;
        private bool Controllable => Player.Controllable;
        private float HoldingAfterJump => Player.PlayerData.Stats.HoldingAfterJump;
        private float DashSpeed => Player.PlayerData.Stats.DashSpeed;
        private float JumpPower => Player.PlayerData.Stats.JumpPower;
        private float MovementSpeed => Player.PlayerData.Stats.MovementSpeed;
        private Animator Animator => Player.Animator;
        private TrailRenderer DashTrail => Player.PlayerData.DashTrailRender;
        private float dashCooldownTimer;
        private float holdingAfterJumpTimer;
        private bool isJumping = false;
        private bool justJumped = false;
        private bool isDashing = false;
        private Vector2 currentVelocity;

        public bool IsJumping => isJumping;
        public bool IsCornerTime => CheckCornerTime();
        public bool ShouldEndDash => IsCornerTime || CheckCollisionEndDash();
        public bool IsGrounded => CheckGrounded();
        public bool AboutToLand => CheckAboutToLand();
        public bool CanJump => !isJumping && !isDashing && IsGrounded;
        private bool CanHoldJump => isJumping && !isDashing;
        private bool CanLand => justJumped && (AboutToLand || IsGrounded);
        private Collider2D BodyCollider => Player.BodyCollider;
        private ParticleSystem JumpParticles => Player.PlayerData.JumpParticles;
        public bool CanDash => !isDashing && dashCooldownTimer <= 0;
        public bool CanRun => !isDashing;
        public bool IsDashing => isDashing;
        public Action OnDashStart { get; set; }
        public Action OnDashEnd { get; set; }
        public float Acceleration { get; set; } = 1;
        public List<LayerMask> WhatEndsDash { get => whatEndsDash; private set => whatEndsDash = value; }

        public void Start()
        {
            DashTrail.widthMultiplier = 0;
        }

        public void Update()
        {
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;
            DoJump();
            DoLand();
        }

        public void FixedUpdate()
        {
            Move();
        }

        // pre: --
        // post: listen walk and dash events
        private void Move()
        {
            if (!Controllable)
                return;

            bool wannaDash = Input.GetButton("Dash");
            if (wannaDash) DoDash();
            else DoRun();

            EndDashCheck();
        }

        private void DoDash()
        {
            if (!CanDash)
                return;

            StartDash();
        }

        // pre: --
        // post: end dash at wall collisions
        private void EndDashCheck()
        {
            if (isDashing && ShouldEndDash)
                EndDash();
        }

        // pre: --
        // post: handles vertical player movement
        private void DoRun()
        {
            if (!CanRun)
                return;

            float horizontal = Input.GetAxis("Horizontal");
            float velocityX = horizontal * MovementSpeed * Acceleration; // (* aceleration => velocityModifier.x)

            if (!IsGrounded) // air drag avoid moving quick in the air 
                velocityX *= AirDrag;

            var newVelocity = new Vector2(velocityX, Body.velocity.y);
            Body.velocity = Vector2.SmoothDamp(Body.velocity, newVelocity, ref currentVelocity, 0.000001f);
            Animator.SetBool(CharacterAnimations.Run, Mathf.Abs(Body.velocity.x) > 0.05f);
        }

        // pre: player just jumped
        // post: character lands triggering landing animation & resets jumping variables
        private void DoLand()
        {
            if (!CanLand)
                return;

            justJumped = false;
            isJumping = false;

            Animator.SetBool(CharacterAnimations.Jumping, isJumping);
        }

        // pre: --
        // post: handles land holding event
        void DoJump()
        {
            if (Input.GetButtonDown("Jump") && CanJump) // button down, first key of jump
            {
                isJumping = true;
                holdingAfterJumpTimer = HoldingAfterJump;
                Body.velocity = new Vector2(Body.velocity.x, JumpPower);
                JumpParticles.Play();
                Animator.SetTrigger(CharacterAnimations.StartJump);
                Animator.SetBool(CharacterAnimations.Jumping, isJumping);
            }
            if (Input.GetButton("Jump") && CanHoldJump) // while jumping
            {
                if (holdingAfterJumpTimer > 0)
                {
                    Body.velocity = new Vector2(Body.velocity.x, JumpPower);
                    holdingAfterJumpTimer -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    justJumped = true;
                }
            }
            if (Input.GetButtonUp("Jump")) // end of jump
            {
                isJumping = false;
                justJumped = true;
            }
        }

        private void StartDash()
        {
            isDashing = true;
            DashTrail.widthMultiplier = 3;
            Animator.SetTrigger(CharacterAnimations.Dash);
            Body.velocity = Vector2.zero;
            Body.gravityScale = 0;
            Body.AddForce(Vector2.right * FacingValue * DashSpeed, ForceMode2D.Impulse);
            OnDashStart?.Invoke();
        }


        // pre: --
        // post: tells you if player is touching 
        private bool CheckGrounded()
        {
            float extra = 0.1f;
            RaycastHit2D ray = Physics2D.BoxCast(BodyCollider.bounds.center, BodyCollider.bounds.size, 0, Vector2.down, extra, WhatIsGround);
            return ray.collider != null;
        }

        // pre: --
        // post: true if touching wall otrw false
        private bool CheckCornerTime()
        {
            return Physics2D.OverlapCircle(WallChecker.origin.position, WallChecker.radius, WhatIsGround);
        }

        private bool CheckCollisionEndDash()
        {
            return WhatEndsDash.FindAll(layer => Body.IsTouchingLayers(layer)).Count >= 1;
        }

        private bool CheckAboutToLand()
        {
            return Body.velocity.y < 0 && Physics2D.OverlapCircle(LandChecker.origin.position, LandChecker.radius, WhatIsGround);
        }


        // pre: callable only by end of dash animation event or wall collision
        public void EndDash()
        {
            if (!isDashing)
                return;

            Body.gravityScale = BaseGravityScale;
            isDashing = false;
            Animator.Rebind(); // resets animator and goes to entry state animator
            DashTrail.widthMultiplier = 0;
            Debug.Log("end dash");
            OnDashEnd?.Invoke();
        }

        /// <sumary>
        /// freze normal control for a certain time applying the impulse.
        /// if anything had change its gravity, 
        // the methods recover its firt local gravity scale
        /// <sumary>
        // TODO: may recover controll after few ms
        public void Impulse(Vector2 power)
        {
            Debug.Log("impulse");
            Body.velocity = Vector2.zero;
            Body.AddForce(power, ForceMode2D.Impulse);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(WallChecker.origin.position, WallChecker.radius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(LandChecker.origin.position, LandChecker.radius);
        }

    }
}

