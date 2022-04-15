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
        [Tooltip("Displacement power on sides while isDashing")][SerializeField] float dashSpeed;
        [Tooltip("Displacement power on sides while jumping")][SerializeField] float jumpForce = 2;
        [Tooltip("How much time player can hold jump bottom")][SerializeField] float holdJump = 0.3f;
        [SerializeField] LayerMask whatIsGround;

        ////////////////////////////////////////////////////////////////////////////////////////////////

        [SerializeField] Vector2 currentVelocity;
        float holdingAfterJump;
        private bool isJumping = false;
        private float horizontal = 0;
        bool justJumped = false;
        bool isDashing = false;
        bool impulsed = false;
        private float baseGravityScale;
        private Vector2 velocityModifyer;
        private float FacingValue => BasePlayer.Instance.FacingValue;
        private float AirDrag => (1f - BasePlayer.Instance.PlayerData.airDrag);
        private float DashCooldown => BasePlayer.Instance.PlayerData.dashCooldown;
        private float DashDuration => BasePlayer.Instance.PlayerData.dashDuration;
        private Rigidbody2D Body => BasePlayer.Instance.Body;
        private Collider2D BodyCollider => BasePlayer.Instance.BodyCollider;
        private bool Controllable => BasePlayer.Instance.Controllable;
        private float HoldingAfterJump => BasePlayer.Instance.PlayerData.holdingAfterJump;
        private float DashSpeed => BasePlayer.Instance.PlayerData.dashSpeed;
        private Animator Animator => BasePlayer.Instance.Animator;
        private float dashCooldownTimer;
        public bool IsOnGround => IsGrounded();
        public bool IsJumping => isJumping;
        public bool CanJump => !isJumping && !isDashing && IsOnGround;
        private bool CanHoldJump => isJumping && !isDashing;
        public bool CanDash => !isDashing && dashCooldownTimer <= 0;
        public bool CanRun => !isDashing;

        public void Start()
        {
            baseGravityScale = Body.gravityScale;
            velocityModifyer = Vector2.one;
        }

        void Update()
        {
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;
            DoJump();
            // DoRun();
        }

        void FixedUpdate()
        {
            // GatherInput();
            Move();
            // Landing();
            // velocityModifyer = Vector2.one;
        }

        // pre: --
        // post: listen player inputs
        private void GatherInput()
        {
            if (!Controllable)
                return;
            horizontal = Input.GetAxis("Horizontal");
            isDashing = Input.GetButton("Dash");
        }

        // pre: GatherInput execution
        private void Move()
        {
            if (!Controllable)
                return;
            bool wannaDash = Input.GetButton("Dash");
            if (wannaDash) DoDash();
            else DoRun();
            // // when Ajax is at the air, we let him take certain control of it's movement
            // var facingValue = BasePlayer.Instance.FacingValue;
            // Debug.Log($"facing value {facingValue}");
            // Debug.Log($"basicSpeed value {basicSpeed}");
            // Debug.Log($"velocityModifier value {velocityModifyer}");
            // float vx = impulsed ?
            // Body.velocity.x + facingValue * basicSpeed * 0.05f
            // : facingValue * basicSpeed * velocityModifyer.x;
            // Body.velocity = new Vector2(vx, Body.velocity.y);
            // basePlayer.Run(Mathf.Abs(Body.velocity.x) > Mathf.Epsilon);
        }

        private void DoDash()
        {
            if (!CanDash)
                return;
            StartCoroutine(DashImpulse());
        }

        private void DoRun()
        {
            if (!CanRun)
                return;

            float horizontal = Input.GetAxis("Horizontal");
            float velocityX = horizontal * basicSpeed; // (* aceleration => velocityModifier.x)

            if (!IsOnGround) // air drag avoid moving quick in the air 
                velocityX *= AirDrag;
            var targetVelocity = new Vector2(velocityX, Body.velocity.y);
            Body.velocity = Vector2.SmoothDamp(Body.velocity, targetVelocity, ref currentVelocity, 0.01f);
        }


        private void Landing()
        {
            if (justJumped && IsOnGround)
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

        // pre: Controllable
        void DoJump()
        {
            if (Input.GetButtonDown("Jump") && CanJump) // button down, first key of jump
            {
                isJumping = true;
                holdingAfterJump = HoldingAfterJump;
                Body.velocity = new Vector2(Body.velocity.x, jumpForce);
                // Animator.SetTrigger("Jump"); // todo: trigger jump animation
            }
            if (Input.GetButton("Jump") && CanHoldJump) // while jumping
            {
                if (holdingAfterJump > 0)
                {
                    Body.velocity = new Vector2(Body.velocity.x, jumpForce);
                    holdingAfterJump -= Time.deltaTime;
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

        private IEnumerator DashImpulse()
        {
            isDashing = true;
            Body.velocity = Vector2.zero;
            Body.gravityScale = 0;
            Body.AddForce(Vector2.right * FacingValue * DashSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(DashDuration);
            Body.gravityScale = baseGravityScale;
            isDashing = false;
        }


        // pre: --
        // post: adds force impulse with facing orientation
        public IEnumerator DashCoroutine(PlayerFacing facing, float duration, System.Action onComplete = null)
        {
            isDashing = true;
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
            isDashing = false;
            if (onComplete != null) onComplete();
        }

        public void Freeze()
        {
            this.Body.velocity = Vector2.zero;
        }

        private bool IsGrounded()
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

