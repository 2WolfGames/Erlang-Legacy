using System;
using System.Collections.Generic;
using Core.Player.Util;
using Core.Utility;
using Core.Utility.Data;
using UnityEngine;


namespace Core.Player.Controller
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] Circle wallChecker;
        [SerializeField] Circle landChecker;
        [SerializeField] LayerMask whatIsGround;
        [SerializeField] List<LayerMask> whatEndsDash;
        [SerializeField] List<LayerMask> whatTriggersLand;
        [SerializeField] private bool isDashing = false;
        [SerializeField] private bool canJump = false;
        private float baseGravityScale;
        private Circle WallChecker { get => wallChecker; set => wallChecker = value; }
        private Circle LandChecker { get => landChecker; set => landChecker = value; }
        private LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
        private PlayerController Player => PlayerController.Instance;
        private float FacingValue => Player.FacingValue;
        private float AirDrag => 1f - Player.PlayerData.Stats.airDrag;
        private Rigidbody2D Body => GetComponent<Rigidbody2D>();
        private bool Controllable => Player.Controllable;
        private float HoldingAfterJump => Player.PlayerData.Stats.holdingAfterJump;
        private float DashSpeed => Player.PlayerData.Stats.dashSpeed;
        private float JumpPower => Player.PlayerData.Stats.jumpPower;
        private float MovementSpeed => Player.PlayerData.Stats.movementSpeed;
        private Animator Animator => Player.Animator;
        private TrailRenderer DashTrail => Player.PlayerData.DashTrailRender;
        private float DashCooldown => Player.PlayerData.Stats.dashCooldown;
        private float dashCooldownTimer;
        private float holdingAfterJumpTimer;
        private bool isJumping = false;
        private bool justJumped = false;
        private Vector2 currentVelocity;
        private bool dashMidJump = false;
        public bool IsJumping => isJumping;
        public bool IsCornerTime => CheckCornerTime();
        public bool ShouldEndDash => IsCornerTime || CheckCollisionEndDash();
        public bool IsGrounded => CheckGrounded();
        public bool AboutToLand => CheckAboutToLand();
        public bool CanJump => !isJumping && !isDashing && IsGrounded;
        private bool CanHoldJump => isJumping && !isDashing;
        private bool CanLand => (justJumped || JustImpulsed) && (AboutToLand || IsGrounded);
        private Collider2D BodyCollider => GetComponent<Collider2D>();
        private ParticleSystem JumpParticles => Player.PlayerData.JumpParticles;
        public bool CanDash => !isDashing && dashCooldownTimer <= 0;
        public bool CanRun => !isDashing;
        public bool IsDashing => isDashing;
        public Action OnDashStart { get; set; }
        public Action OnDashFinish { get; set; }
        public float Acceleration { get; set; } = 1;
        public bool JustImpulsed { get; set; }
        public List<LayerMask> WhatEndsDash { get => whatEndsDash; private set => whatEndsDash = value; }
        public List<LayerMask> WhatTriggersLand { get => whatTriggersLand; private set => whatTriggersLand = value; }


        public void Start()
        {
            DashTrail.widthMultiplier = 0;
            baseGravityScale = Body.gravityScale;
        }

        public void Update()
        {
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;

            canJump = CanJump;

            if (!Controllable) // animation that can be called from any state may not let recover event work as spected
                return;

            DoJump();
            DoLand();
        }

        public void FixedUpdate()
        {
            FaceDirection();
            Move();
        }

        // desc: changes local player scale in order to align to user input
        public void FaceDirection()
        {
            if (!Controllable)
                return;

            Vector3 scale = transform.localScale;
            scale.x = FacingValue;
            transform.localScale = scale;
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

            CheckDashComplitness();
        }

        private void DoDash()
        {
            if (!CanDash)
                return;

            StartDashing();
        }

        // pre: --
        // post: end dash at wall collisions
        private void CheckDashComplitness()
        {
            if (isDashing && ShouldEndDash)
                StopDashing();
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
            Animator.SetBool(CharacterAnimations.Running, Mathf.Abs(Body.velocity.x) > 0.05f);
        }

        // pre: player just jumped
        // post: character lands triggering landing animation & resets jumping variables
        private void DoLand()
        {
            if (!CanLand)
                return;

            justJumped = false;
            isJumping = false;
            JustImpulsed = false;

            Animator.SetBool(CharacterAnimations.Jumping, isJumping);
        }

        // pre: --
        // post: handles land holding event
        void DoJump()
        {
            Action endJump = () =>
            {
                isJumping = false;
                justJumped = true;
            };

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
                if (dashMidJump)
                {
                    endJump();
                    dashMidJump = false;
                    return;
                }

                if (holdingAfterJumpTimer > 0)
                {
                    Body.velocity = new Vector2(Body.velocity.x, JumpPower);
                    holdingAfterJumpTimer -= Time.deltaTime;
                }
                else endJump();
            }
            if (Input.GetButtonUp("Jump")) // end of jump
                endJump();
        }

        // pre: can do dash
        private void StartDashing()
        {
            if (isJumping)
                dashMidJump = true;

            isDashing = true;
            DashTrail.widthMultiplier = 3;
            Animator.SetTrigger(CharacterAnimations.Dash);
            Body.velocity = Vector2.zero;
            Body.gravityScale = 0;
            Body.AddForce(Vector2.right * FacingValue * DashSpeed, ForceMode2D.Impulse);
            OnDashStart?.Invoke();
        }

        // pre: --
        // post: checks if player is touching ground
        private bool CheckGrounded()
        {
            float extra = 0.1f;
            RaycastHit2D ray = Physics2D.BoxCast(BodyCollider.bounds.center, BodyCollider.bounds.size, 0, Vector2.down, extra, WhatIsGround);
            return ray.collider != null;
        }

        // pre: --
        // post: check if player is near and facing wall using wall checker object
        private bool CheckCornerTime()
        {
            return Physics2D.OverlapCircle(WallChecker.origin.position, WallChecker.radius, WhatIsGround);
        }

        // pre: --
        // post: check if currently touching one of those layers that ends dash
        //      please make sure to NOT add ground layer in `WhatEndsDash`, just thouse object
        //      that collide with player like (Springboard)
        private bool CheckCollisionEndDash()
        {
            return WhatEndsDash.FindAll(layer => Body.IsTouchingLayers(layer)).Count >= 1;
        }

        // pre: --
        // post: usufull to trigger land animation when player comes from impulsed or jumped events
        private bool CheckAboutToLand()
        {
            bool landing = false;
            foreach (var layer in WhatTriggersLand)
            {
                landing = Physics2D.OverlapCircle(LandChecker.origin.position, LandChecker.radius, layer);
                if (landing)
                    break;
            }
            return landing && Body.velocity.y < 0;
        }

        // pre: callable only by end of dash animation event or wall collision
        // post: resets values changes from StartDashing fn, resets animator to go to idle state
        public void StopDashing()
        {
            if (!isDashing)
                return;

            dashCooldownTimer = DashCooldown;
            Body.gravityScale = baseGravityScale;
            isDashing = false;
            DashTrail.widthMultiplier = 0;
            FreezeVelocity();
            OnDashFinish?.Invoke();
        }

        /// <sumary>
        /// freze normal control for a certain time applying the impulse.
        /// if anything had change its gravity, 
        /// if anything had change its gravity, 
        /// if anything had change its gravity, 
        // the methods recover its firt local gravity scale
        /// <sumary>
        // TODO: may recover controll after few ms instead of inmediate contorll
        public void Impulse(Vector2 power)
        {
            Body.velocity = Vector2.zero;
            Body.AddForce(power, ForceMode2D.Impulse);
            JustImpulsed = true;
            Animator.SetTrigger(CharacterAnimations.StartJump);
            Animator.SetBool(CharacterAnimations.Jumping, true);
        }

        // pre: --
        // post: freezes current movement velocity
        public void FreezeVelocity()
        {
            Body.velocity = Vector2.zero;
        }

        public void OnDrawGizmosSelected()
        {
            Color rayColor = IsGrounded ? Color.green : Color.red;
            float extra = 0.1f;
            Debug.DrawRay(BodyCollider.bounds.center + new Vector3(BodyCollider.bounds.extents.x, 0), Vector2.down * (BodyCollider.bounds.extents.y + extra), rayColor);
            Debug.DrawRay(BodyCollider.bounds.center - new Vector3(BodyCollider.bounds.extents.x, 0), Vector2.down * (BodyCollider.bounds.extents.y + extra), rayColor);
            Debug.DrawRay(BodyCollider.bounds.center - new Vector3(BodyCollider.bounds.extents.x, BodyCollider.bounds.extents.y + extra), Vector2.right * (2 * BodyCollider.bounds.extents.x), rayColor);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(WallChecker.origin.position, WallChecker.radius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(LandChecker.origin.position, LandChecker.radius);
        }
    }
}

