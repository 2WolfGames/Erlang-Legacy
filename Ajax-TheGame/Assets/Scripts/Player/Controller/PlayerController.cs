using System.Collections;
using Core.Player.Data;
using Core.Player.Util;
using Core.Shared;
using Core.Shared.Enum;
using Core.Util;
using UnityEngine;

namespace Core.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] bool shakeCameraOnHurt = true;
        [SerializeField] private bool controllable = true;
        [SerializeField] private bool inRecoverProcess = false;
        [SerializeField] private bool isProtected = true;
        [SerializeField] PlayerData playerData;
        private AbilityController abilityController => GetComponent<AbilityController>();
        private MovementController movementController => GetComponent<MovementController>();
        private FacingController facingController => GetComponent<FacingController>();
        private Protectable protectable => GetComponent<Protectable>();
        private bool blockingUI;
        public bool CanBeHit => protectable.CanBeHit;
        public int FacingValue => facingController.FacingToInt;
        public PlayerData PlayerData { get => playerData; private set => playerData = value; }
        public bool IsGrounded => movementController.IsGrounded;
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

        public void Update()
        {
            isProtected = protectable.IsProtected;
        }

        // pre: --
        // post: enables scripts that make damage
        //      and sets infinite protection
        private void OnDashStart()
        {
            protectable.SetProtection(float.PositiveInfinity);
            controllable = false;
            abilityController.ActiveDashDamage();
        }

        // post: disable scripts that make damage
        //       and sets protection to false in case
        //       there is no recover process running
        public void OnDashCompletes()
        {
            if (!inRecoverProcess) // respects recover process
                protectable.SetProtection(0f);

            controllable = true;
            abilityController.DeactiveDashDamage();
            movementController.StopDashing();
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
        private void OnBlockUI()
        {
            Debug.Log("unfreeze game play");
        }

        // pre: --
        // post: applies damage to player
        public void OnCollision(GameObject other, int damage = 1)
        {
            if (protectable.IsProtected)
                return;

            Hurt(damage, other);
        }

        // pre: --
        // post: applies damage to player
        public void Hurt(int damage, GameObject other)
        {
            if (protectable.IsProtected)
                return;

            movementController.FreezeVelocity();

            ComputeSideHurtAnimation(other.transform);

            if (shakeCameraOnHurt)
                CameraController.Instance.ShakeCamera();

            OnRecoverStart();
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
            protectable.SetProtection(float.PositiveInfinity);
            Animator.SetBool(CharacterAnimations.Blink, true);
            controllable = false;
        }

        // desc: to be called at end of recover animations with and event
        public void OnRecoverComplete()
        {
            controllable = true;
            StartCoroutine(AfterHurtAnimation());
        }

        // pre:  after hurt animations & recover process running
        // post: trigger animations & and resets protection after few seconds
        //       if no dashing process is running, set character unprotected
        private IEnumerator AfterHurtAnimation()
        {
            var timeout = PlayerData.Stats.RecoverTimeoutAfterHit;
            yield return new WaitForSeconds(timeout);
            inRecoverProcess = false;

            if (!movementController.IsDashing) // respects dashing protection
                protectable.SetProtection(0);

            Animator.SetBool(CharacterAnimations.Blink, false);
        }
    }
}

