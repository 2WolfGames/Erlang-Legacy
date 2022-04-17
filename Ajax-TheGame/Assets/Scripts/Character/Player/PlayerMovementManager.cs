using System.Collections;
using UnityEngine;
using System;

using Core.Shared.Enum;

// todo: rename movement controller to player movement manager
// todo: instead of using player manager functions use lambda function setted at awake 
namespace Core.Character.Player
{
    public class PlayerMovementManager : MonoBehaviour
    {
        [SerializeField] Vector2 currentVelocity;
        private float BaseGravityScale => Player.BaseGravityScale;
        private Vector2 velocityModifyer;
        private BasePlayer Player => BasePlayer.Instance;
        private float FacingValue => Player.FacingValue;
        private float AirDrag => (1f - Player.PlayerData.airDrag);
        private float DashCooldown => Player.PlayerData.dashCooldown;
        private Rigidbody2D Body => Player.Body;
        private Collider2D BodyCollider => Player.BodyCollider;
        private bool Controllable => Player.Controllable;
        private float HoldingAfterJump => Player.PlayerData.holdingAfterJump;
        private float DashSpeed => Player.PlayerData.dashSpeed;
        private float JumpPower => Player.PlayerData.jumpPower;
        private float MovementSpeed => Player.PlayerData.movementSpeed;
        private Animator Animator => Player.Animator;
        private bool IsOnGround => Player.IsGrounded;
        private TrailRenderer DashTrail => Player.PlayerData.dashTrailRender;
        private float dashCooldownTimer;
        private float holdingAfterJumpTimer;
        private bool isJumping = false;
        private bool justJumped = false;
        private bool isDashing = false;
        private bool impulsed = false;
        public bool IsJumping => isJumping;
        public bool CanJump => !isJumping && !isDashing && IsOnGround;
        private bool CanHoldJump => isJumping && !isDashing;
        public bool CanDash => !isDashing && dashCooldownTimer <= 0;
        public bool CanRun => !isDashing;
        public bool IsDashing => isDashing;
        private bool CanLand => justJumped && IsOnGround;
        private ParticleSystem JumpParticles => Player.PlayerData.jumpParticles;

        public Action OnDashStart;
        public Action OnDashEnd;

        public void Start()
        {
            velocityModifyer = Vector2.one;
            DashTrail.widthMultiplier = 0;
        }

        void Update()
        {
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;
            DoJump();
            DoLand();
        }

        void FixedUpdate()
        {
            Move();
            // Landing();
            // velocityModifyer = Vector2.one;
        }

        // pre: GatherInput execution
        private void Move()
        {
            if (!Controllable)
                return;
            bool wannaDash = Input.GetButton("Dash");
            if (wannaDash) DoDash();
            else DoRun();
        }

        private void DoDash()
        {
            if (!CanDash)
                return;
            StartDash();
        }

        // pre: player should be controllable
        private void DoRun()
        {
            if (!CanRun)
                return;
            float horizontal = Input.GetAxis("Horizontal");
            float velocityX = horizontal * MovementSpeed; // (* aceleration => velocityModifier.x)

            if (!IsOnGround) // air drag avoid moving quick in the air 
                velocityX *= AirDrag;

            var newVelocity = new Vector2(velocityX, Body.velocity.y);
            Body.velocity = Vector2.SmoothDamp(Body.velocity, newVelocity, ref currentVelocity, 0.0001f);
            Animator.SetBool(CharacterAnimations.Run, Mathf.Abs(Body.velocity.x) > 0.05f);
        }

        private void DoLand()
        {
            if (!CanLand) return;
            justJumped = false;
            isJumping = false;
            Animator.SetBool(CharacterAnimations.Jumping, isJumping);
        }

        /// <sumary>
        /// freze normal control for a certain time applying the impulse.
        /// if anything had change its gravity, 
        // the methods recover its firt local gravity scale
        /// <sumary>
        public void ImpulseUp(float force)
        {
            this.Body.gravityScale = BaseGravityScale;
            impulsed = true;
            // Freeze();
            Body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        /// <sumary>
        /// freze normal control for a certain time applying the impulse.
        /// if anything had change its gravity, 
        // the methods recover its firt local gravity scale
        /// <sumary>
        public void Impulse(Vector2 impulse)
        {
            this.Body.gravityScale = BaseGravityScale;
            impulsed = true;
            // Freeze();
            Body.AddForce(impulse, ForceMode2D.Impulse);
        }

        // pre: Controllable
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

        // pre: callable only by end of dash animation event or wall collision
        public void EndDash()
        {
            if (!isDashing)
                return;
            Body.gravityScale = BaseGravityScale;
            isDashing = false;
            Animator.Rebind(); // resets animator and goes to entry state animator
            DashTrail.widthMultiplier = 0;
            OnDashEnd?.Invoke();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            // trigger effect ends when you had collide with something
            if (impulsed)
            {
                impulsed = false;
            }
        }

        //pre: -
        //post: velocity modifyer is updated with the values 
        //that are going to modify the velocity on ONE fixedUpdate
        public void ModifyVelocity(Vector2 velocityModifyer)
        {
            this.velocityModifyer = velocityModifyer;
        }

    }
}

