using System.Collections;
using Core.Player.Data;
using Core.Player.Util;
using Core.Shared;
using Core.Shared.Enum;
using Core.UI.LifeBar;
using Core.Utility;
using UnityEngine;

namespace Core.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] bool shakeCameraOnHurt = true;
        [SerializeField] private bool controllable = true;
        [SerializeField] public bool inRecoverProcess = false;
        [SerializeField] PlayerData playerData;
        private AbilityController abilityController => GetComponent<AbilityController>();
        private MovementController movementController => GetComponent<MovementController>();
        private FacingController facingController => GetComponent<FacingController>();
        private Protectable protectable => GetComponent<Protectable>();
        private bool isDashing => movementController.IsDashing;
        private float recoverTimeoutAfterHit => playerData.Stats.recoverTimeoutAfterHit;
        private bool blockingUI;
        public bool CanBeHit => protectable.CanBeHit;
        public int FacingValue => facingController.FacingToInt;
        public PlayerData PlayerData { get => playerData; private set => playerData = value; }
        public bool IsGrounded => movementController.IsGrounded;
        public Stats Stats => playerData.Stats;
        public bool BlockingUI
        {
            get => blockingUI;
            set
            {
                blockingUI = value;
                if (blockingUI)
                    OnBlockUI();
                else OnUnblockUI();
            }
        }

        public bool Controllable
        {
            get => controllable && !BlockingUI;
            set => controllable = value;
        }

        public Collider2D BodyCollider => GetComponent<Collider2D>();
        public Rigidbody2D Body => GetComponent<Rigidbody2D>();
        public Animator Animator => GetComponentInChildren<Animator>();
        public static PlayerController Instance { get; private set; }

        protected void Awake()
        {
            var matches = FindObjectsOfType<PlayerController>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;

            movementController.OnDashStart += OnDashStart;
        }

        // post: disable scripts that make damage
        //       and sets protection to false in case
        //       there is no recover process running
        public void OnDashComplete()
        {
            movementController.StopDashing();
            AfterDashComplete();
        }

        // post: enables scripts that make damage
        //      and sets infinite protection
        private void OnDashStart()
        {
            controllable = false;
            protectable.SetProtection(ProtectionType.INFINITE);
            abilityController.ActiveDashDamage();
        }

        private void AfterDashComplete()
        {
            controllable = true;
            abilityController.DeactiveDashDamage();

            if (!inRecoverProcess)
                protectable.SetProtection(ProtectionType.NONE);
        }

        // pre: called by some function that stunds player (called by hit animation)
        // post: enable scripts & returns normal game constants
        private void OnUnblockUI()
        {
            Debug.Log("freeze game play");
            // reset global game constant to normal state if needed...
        }

        // pre: (called by hit end animation)
        // post: detach component logic scripts & freeze player
        // PROP: instead of freezing player we can make time slow 
        public void OnBlockUI()
        {
            Debug.Log("unfreeze game play");
        }

        public void Heal()
        {
            // TODO: trigger heal animation && vfx
            playerData.Health.HP++;
        }

        public void Heal(int hp)
        {
            // TODO: trigger heal animation && vfx
            playerData.Health.HP += hp;
        }

        // pre: --
        // post: applies damage to player. 1 unit of damage represent 1 unit of life taken
        public void Hurt(int damage, GameObject other)
        {
            if (protectable.IsProtected)
                return;

            movementController.FreezeVelocity();
            TakeLifes(damage);
            ComputeSideHurtAnimation(other.transform);

            if (shakeCameraOnHurt)
                CameraManager.Instance?.ShakeCamera();

            OnRecoverStart();
        }

        private void TakeLifes(int damage)
        {
            LifeBarController.Instance?.LoseLifes(damage);
            playerData.Health.HP = playerData.Health.HP - damage;
        }

        private void ComputeSideHurtAnimation(Transform other)
        {
            Side side = Function.CollisionSide(transform, other.transform);

            if (side == Side.Back)
                Animator.SetTrigger(CharacterAnimations.BackHurt);
            else Animator.SetTrigger(CharacterAnimations.FrontHurt);
        }

        // pre: called at first key frame hit (backward & forward) animations       
        private void OnRecoverStart()
        {
            if (protectable.IsProtected)
                return;

            inRecoverProcess = true;
            protectable.SetProtection(ProtectionType.INFINITE);
            Animator.SetBool(CharacterAnimations.Blink, true);
            controllable = false;
        }

        // desc: to be called at end of recover animations with and event
        public void OnRecoverComplete()
        {
            controllable = true;
            StartCoroutine(AfterRecoverComplete());
        }

        // pre:  after hurt animations & recover process running
        // post: trigger animations & and resets protection after few seconds
        //       if no dashing process is running, set character unprotected
        private IEnumerator AfterRecoverComplete()
        {
            yield return new WaitForSeconds(recoverTimeoutAfterHit);

            inRecoverProcess = false;

            if (!isDashing) // respects dashing protection
                protectable.SetProtection(ProtectionType.NONE);

            Animator.SetBool(CharacterAnimations.Blink, false);
        }

        //pre: --
        //post: faces player = playerFacing
        public void SetFacing(PlayerFacing playerFacing)
        {
            facingController.SetFacing(playerFacing);
            movementController.FaceDirection();
        }

        public void OnDie()
        {
            controllable = false;
            Animator.SetTrigger(CharacterAnimations.Die);
        }

        public void InvokeRay()
        {
            abilityController.InvokeRay();
        }

        public void OnRayStarts()
        {
            controllable = false;
            Body.velocity = Vector3.zero;
            Body.gravityScale = 0;
        }

        public void OnRayEnd()
        {
            controllable = true;
            Body.gravityScale = 10;
        }

        public void FreezeMovement()
        {
            movementController.FreezeVelocity();
        }

    }
}

