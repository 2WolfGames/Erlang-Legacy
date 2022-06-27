using System.Collections;
using Core.Player.Data;
using Core.Player.Util;
using Core.Shared;
using Core.Shared.Enum;
using Core.Utility;
using UnityEngine;
using Core.UI;

namespace Core.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public bool shakeCameraOnHurt = true;
        public bool inRecoverProcess = false;
        public PlayerData playerData;
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
        public bool Punching => abilityController.Punching;
        private bool controllable = true;
        private AbilityController abilityController => GetComponent<AbilityController>();
        private MovementController movementController => GetComponent<MovementController>();
        private FacingController facingController => GetComponent<FacingController>();
        private Protectable protectable => GetComponent<Protectable>();
        private bool isDashing => movementController.IsDashing;
        private float recoverTimeoutAfterHit => playerData.Stats.recoverTimeoutAfterHit;
        private bool blockingUI;
        private float baseGravityScale = 1f;

        protected void Awake()
        {
            var matches = FindObjectsOfType<PlayerController>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;

            movementController.OnDashStart += OnDashStart;
            baseGravityScale = Body.gravityScale;
        }

        private void Start()
        {
            if (!IsAlive()) OnDie();
        }

        public void OnDashComplete()
        {
            movementController.StopDashing();
            abilityController.OnDashComplete();

            controllable = true;

            if (!inRecoverProcess)
                protectable.SetProtection(ProtectionType.NONE);
        }

        // post: enables scripts that make damage
        //      and sets infinite protection
        private void OnDashStart()
        {
            controllable = false;
            protectable.SetProtection(ProtectionType.INFINITE);
            abilityController.OnDashStart();
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
            if (!CanBeHit || !IsAlive())
                return;

            ShakeCamera();
            Freeze();
            ResetAbilities();
            TakeLifes(damage);
            ComputeSideHurtAnimation(other.transform);
            AfterHurt();
        }

        private void ShakeCamera()
        {
            if (shakeCameraOnHurt)
                CameraManager.Instance?.ShakeCamera();
        }

        private void AfterHurt()
        {
            if (IsAlive())
                OnRecoverStart();
            else OnDie();
        }

        private void ResetAbilities()
        {
            abilityController.PunchEnd();
            OnDashComplete();
        }

        private void TakeLifes(int damage)
        {
            playerData.Health.HP = playerData.Health.HP - damage;
        }

        private void ComputeSideHurtAnimation(Transform other)
        {
            Side side = Function.RelativeCollisionSide(transform, other.transform);

            if (side == Side.Back)
                Animator.SetTrigger(CharacterAnimations.BackHurt);
            else Animator.SetTrigger(CharacterAnimations.FrontHurt);
        }

        // pre: called at first key frame hit (backward & forward) animations       
        private void OnRecoverStart()
        {
            Animator.SetBool(CharacterAnimations.Blink, true);
            inRecoverProcess = true;
            protectable.SetProtection(ProtectionType.INFINITE);
            controllable = false;
        }

        // desc: to be called at end of recover animations with and event
        public void OnRecoverComplete()
        {
            if (!inRecoverProcess)
                return;

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
        //post: faces player = Face
        public void SetFacing(Face Face)
        {
            facingController.SetFacing(Face);
            movementController.FaceDirection();
        }

        public void OnDie()
        {
            controllable = false;
            Freeze();
            Animator.SetTrigger(CharacterAnimations.Die);
        }

        public void Freeze()
        {
            Body.velocity = Vector2.zero;
        }

        public void ZeroGravity()
        {
            Body.gravityScale = 0;
        }

        public void BaseGravity()
        {
            Body.gravityScale = baseGravityScale;
        }

        public bool IsAlive()
        {
            return playerData.Health.HP > 0;
        }

        public bool IsDead()
        {
            return !IsAlive();
        }

        public void AdquireAbility(Ability ability)
        {
            if (AdquiredAbility(ability))
            {
                Debug.LogWarning($"Ability - {ability} - already adquired");
            }
            else
            {
                SaveAdquiredAbility(ability);
                abilityController.AdquireAbility(ability);
                PowersPanelManager.Instance?.ManagePowersVisibility();
            }
        }

        public bool AdquiredAbility(Ability ability)
        {
            return abilityController.AdquiredAbility(ability);
        }

        private void SaveAdquiredAbility(Ability ability)
        {
            Debug.LogWarning($"PlayerController@SaveAdquiredAbility: Method not implemented yet");
            // TODO: save ability to file
        }
    }
}

