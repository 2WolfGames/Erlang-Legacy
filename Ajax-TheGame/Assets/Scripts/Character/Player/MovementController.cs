using System.Collections;
using UnityEngine;

using Core.Shared.Enum;

// todo: rename movement controller to player movement manager
// todo: instead of using player manager functions use lambda function setted at awake 
namespace Core.Character.Player
{
    public class MovementController : MonoBehaviour
    {
        [Header("Configurations")]
        [Tooltip("Displacement power on sides while running")][SerializeField] float basicSpeed;
        [Tooltip("Displacement power on sides while dashing")][SerializeField] float dashSpeed;
        [Tooltip("Displacement power on sides while jumping")][SerializeField] float jumpForce = 2;
        [Tooltip("How much time player can hold jump bottom")][SerializeField] float holdJump = 0.3f;
        [SerializeField] LayerMask whatIsGround;

        ////////////////////////////////////////////////////////////////////////////////////////////////

        float jumpHoldTimer;
        private bool isJumping = false;
        bool justJumped = false;
        bool dashing = false;
        bool impulsed = false;
        private float baseGravityScale;
        private Vector2 velocityModifyer;
        private float JumpCooldown => BasePlayer.Instance.PlayerData.jumpCooldown;
        private Rigidbody2D Body => BasePlayer.Instance.Body;
        private Collider2D BodyCollider => BasePlayer.Instance.BodyCollider;
        private bool IsDashing => BasePlayer.Instance.IsDashing;
        private bool IsControllable => BasePlayer.Instance.Controllable;
        private float JumpHoldDuration => BasePlayer.Instance.PlayerData.jumpHoldDuration;
        private Animator Animator => BasePlayer.Instance.Animator;
        private float jumpTimer;
        public bool CanMove => IsControllable && !IsDashing;
        public bool IsGrounded => CheckIfGrounded();
        public bool IsJumping
        {
            get => isJumping;
            private set { isJumping = value; }
        }
        public bool CanJump => !IsJumping && jumpTimer <= 0 && CanMove && IsGrounded;

        void Awake()
        {
            baseGravityScale = Body.gravityScale;
            velocityModifyer = Vector2.one;
        }

        private void ResetJumpTimer()
        {
            this.jumpTimer = JumpCooldown;
        }

        void Update()
        {
            if (jumpTimer > 0)
                jumpTimer -= Time.deltaTime;

            SmoothJump();
        }

        void FixedUpdate()
        {
            Moving();
            Landing();
            velocityModifyer = Vector2.one;
        }

        private void Moving()
        {
            if (!CanMove) return;
            // when Ajax is at the air, we let him take certain control of it's movement
            var facingValue = BasePlayer.Instance.FacingValue;
            float vx = impulsed ?
            Body.velocity.x + facingValue * basicSpeed * 0.05f
            : facingValue * basicSpeed * velocityModifyer.x;
            Body.velocity = new Vector2(vx, Body.velocity.y);
            // basePlayer.Run(Mathf.Abs(Body.velocity.x) > Mathf.Epsilon);
        }

        private void Landing()
        {
            if (justJumped && IsGrounded)
            {
                // basePlayer.Land();
                justJumped = false;
            }
        }

        /// <sumary>
        /// freze normal control for a certain time applying the impulse.
        /// if anything had change its gravity, 
        // the methods recover its firt local gravity scale
        /// <sumary>
        public void ImpulseUp(float force)
        {
            this.Body.gravityScale = baseGravityScale;
            impulsed = true;
            Freeze();
            Body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        /// <sumary>
        /// freze normal control for a certain time applying the impulse.
        /// if anything had change its gravity, 
        // the methods recover its firt local gravity scale
        /// <sumary>
        public void Impulse(Vector2 impulse)
        {
            this.Body.gravityScale = baseGravityScale;
            impulsed = true;
            Freeze();
            Body.AddForce(impulse, ForceMode2D.Impulse);
        }

        void SmoothJump()
        {
            if (Input.GetButtonDown("Jump") && CanJump) // button down, first key of jump
            {
                IsJumping = true;
                jumpHoldTimer = JumpHoldDuration;
                Body.velocity = new Vector2(Body.velocity.x, jumpForce);
                // Animator.SetTrigger("Jump"); // todo: trigger jump animation
            }

            if (Input.GetButton("Jump") && IsJumping) // while jumping
            {
                if (jumpHoldTimer > 0)
                {
                    Body.velocity = new Vector2(Body.velocity.x, jumpForce);
                    jumpHoldTimer -= Time.deltaTime;
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

        // pre: --
        // post: adds force impulse with facing orientation
        public IEnumerator DashCoroutine(PlayerFacing facing, float duration, System.Action onComplete = null)
        {
            dashing = true;
            float gravityScale = this.Body.gravityScale;
            Freeze();
            this.Body.gravityScale = 0;
            var direction = facing == PlayerFacing.Left ? -1 : 1;
            this.Body.AddForce(new Vector2(dashSpeed * direction, 0f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(duration);

            // avoids stack when you had dash
            // and them Ajax was trigger by impulse effect
            if (!impulsed)
            {
                Freeze();
            }
            this.Body.gravityScale = gravityScale;
            dashing = false;
            if (onComplete != null) onComplete();
        }

        public void Freeze()
        {
            this.Body.velocity = Vector2.zero;
        }

        private bool CheckIfGrounded()
        {
            float extra = 0.1f;
            RaycastHit2D ray = Physics2D.BoxCast(BodyCollider.bounds.center, BodyCollider.bounds.size, 0, Vector2.down, extra, whatIsGround);
            bool grounded = ray.collider != null;
            Color rayColor = grounded ? Color.green : Color.red;

            Debug.DrawRay(BodyCollider.bounds.center + new Vector3(BodyCollider.bounds.extents.x, 0), Vector2.down * (BodyCollider.bounds.extents.y + extra), rayColor);
            Debug.DrawRay(BodyCollider.bounds.center - new Vector3(BodyCollider.bounds.extents.x, 0), Vector2.down * (BodyCollider.bounds.extents.y + extra), rayColor);
            Debug.DrawRay(BodyCollider.bounds.center - new Vector3(BodyCollider.bounds.extents.x, BodyCollider.bounds.extents.y + extra), Vector2.right * (2 * BodyCollider.bounds.extents.x), rayColor);

            return grounded;
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

