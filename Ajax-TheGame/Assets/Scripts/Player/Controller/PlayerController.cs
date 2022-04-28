using System.Collections;
using UnityEngine;
using Core.Player.Data;
using Core.Player.Util;
using Core.Shared;
using Core.Shared.Enum;
using Core.Util;
using Core.UI.LifeBar;

namespace Core.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerData playerData;
        private AbilityController AbilityController => GetComponent<AbilityController>();
        private MovementController MovementController => GetComponent<MovementController>();
        private FacingController FacingController => GetComponent<FacingController>();
        private Protectable Protectable => GetComponent<Protectable>();
        [SerializeField] private bool controllable = true;
        [SerializeField] private bool inRecoverProcess = false;
        [SerializeField] private bool isProtected;
        private bool blockingUI;
        public bool CanBeHit => Protectable.CanBeHit;
        public int FacingValue => FacingController.FacingToInt;
        public PlayerData PlayerData { get => playerData; private set => playerData = value; }
        public bool IsGrounded => MovementController.IsGrounded;
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

        public void Update()
        {
            isProtected = Protectable.IsProtected;
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


        public void Awake()
        {
            var matches = FindObjectsOfType<PlayerController>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else
                Instance = this;

            MovementController.OnDashStart += OnDashStart;
        }

        // pre: --
        // post: enables scripts that make damage
        //      and sets infinite protection
        private void OnDashStart()
        {
            Protectable.SetProtection(float.PositiveInfinity);
            controllable = false;
            AbilityController.ActiveDashDamage();
        }

        // post: disable scripts that make damage
        //       and sets protection to false in case
        //       there is no recover process running
        public void OnDashCompletes()
        {
            if (!inRecoverProcess) // respects recover process
                Protectable.SetProtection(0f);

            controllable = true;
            AbilityController.DeactiveDashDamage();
            MovementController.StopDashing();
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
            if (Protectable.IsProtected)
                return;

            Hurt(damage, other);
        }

        // pre: --
        // post: applies damage to player
        public void Hurt(int lifesPlayerLoses, GameObject other)
        {
            if (Protectable.IsProtected)
                return;

            Side side = Function.CollisionSide(transform, other.transform);

            /*Changes*/
            LifeBarController.Instance?.LoseLifes(lifesPlayerLoses);
            playerData.Health.HP = playerData.Health.HP - lifesPlayerLoses;
            /*Changes*/

            if (side == Side.Back)
                Animator.SetTrigger(CharacterAnimations.BackHurt);
            else Animator.SetTrigger(CharacterAnimations.FrontHurt);

            MovementController.FreezeVelocity();

            OnRecoverStart();
        }

        // pre: called at first key frame hit (backward & forward) animations       
        private void OnRecoverStart()
        {
            if (Protectable.IsProtected)
                return;

            inRecoverProcess = true;
            Protectable.SetProtection(float.PositiveInfinity);
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

            if (!MovementController.IsDashing) // respects dashing protection
                Protectable.SetProtection(0);

            Animator.SetBool(CharacterAnimations.Blink, false);
        }
    }
}

