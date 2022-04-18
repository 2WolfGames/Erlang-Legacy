using UnityEngine;
using Core.Shared;
using Core.Shared.Enum;
using Core.Combat.Projectile;
using Core.Util;

using Core.Player.Data;
using System;
using System.Collections;
using Core.Player.Util;

namespace Core.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerData playerData;
        private AbilityController AbilityController => GetComponent<AbilityController>();
        private MovementController MovementController => GetComponent<MovementController>();
        private FacingController FacingController => GetComponent<FacingController>();
        private Protectable Protectable => GetComponent<Protectable>();
        private bool controllable = true;
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
        public bool Controllable { get => controllable && !BlockingUI; set => controllable = value; }
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


            AbilityController.OnRayStart += OnRayStart;

            PlayerData.DamageArea.Dash.OnHit += OnDashHit;
            PlayerData.DamageArea.Dash.SetEnabled(false);
            PlayerData.DamageArea.Punch.SetEnabled(false);
            PlayerData.Projectile.Projectile.OnHit += OnRayProjectileHit;

            MovementController.OnDashStart += OnDashStart;
            MovementController.OnDashEnd += OnDashEnd;
        }

        // pre: --
        // post: disable scripts that make damage
        //      and activates listening from keyboard
        private void OnDashEnd()
        {
            FacingController.enabled = true;
            PlayerData.DamageArea.Dash.SetEnabled(false);
        }

        // pre: --
        // post: enable scripts that make damage
        //      and avoid listening from keyboard
        private void OnDashStart()
        {
            FacingController.enabled = false;
            PlayerData.DamageArea.Dash.SetEnabled(true);
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
            if (Protectable.IsProtected) return;
            Hurt(damage, other);
        }

        // pre: --
        // post: applies damage to player
        public void Hurt(int damage, GameObject other)
        {
            if (Protectable.IsProtected) return;

            Protectable.ResetProtection();

            Side side = Function.CollisionSide(transform, other.transform);

            if (side == Side.Back)
                Animator.SetTrigger(CharacterAnimations.BackHurt);
            else Animator.SetTrigger(CharacterAnimations.FrontHurt);

            OnRecoverStart();
            SetTimeOut(OnRecoverComplete, Protectable.ProtectionDuration); // TODO: work in recover

        }

        private void OnRecoverStart()
        {
            Animator.SetBool(CharacterAnimations.Blink, true);
            MovementController.enabled = false;
        }

        private void OnRecoverComplete()
        {
            Animator.SetBool(CharacterAnimations.Blink, false);
            MovementController.enabled = true;
        }

        public void SetTimeOut(Action function, float seconds)
        {
            StartCoroutine(SetTimeoutCoroutine(function, seconds));
        }

        private IEnumerator SetTimeoutCoroutine(Action function, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            function.Invoke();
        }

        private void OnDashHit(Collider2D enemy)
        {
            Debug.Log("Hitting enemy at dash");
        }

        private void OnRayProjectileHit(Collider2D enemy)
        {
            Debug.Log("Hitting enemy at ray");
        }

        private void OnPunchHit(Collider2D enemy)
        {
            Debug.Log("Hitting enemy at punch");
        }

        // pre: --
        // post: instanciate a ray prefab that will destroy itself in n seconds
        private void OnRayStart()
        {

        }

    }
}

