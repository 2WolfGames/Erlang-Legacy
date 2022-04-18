using UnityEngine;
using Core.Shared;
using Core.Shared.Enum;
using Core.Combat.Projectile;
using Core.Util;

using Core.Player.Data;


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
                    OnUncontrollable();
                else OnControllable();
            }
        }
        public bool Controllable
        {
            get => controllable && !BlockingUI;
            set
            {
                controllable = value;
                if (!controllable)
                    OnUncontrollable();
                else OnControllable();
            }
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


            AbilityController.OnTriggerRay += OnTriggerRay;

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
        private void OnControllable()
        {
            Debug.Log("freeze game play");
            // reset global game constant to normal state if needed...
        }

        // pre: (called by hit end animation)
        // post: detach component logic scripts & freeze player
        // PROP: instead of freezing player we can make time slow 
        private void OnUncontrollable()
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
            // ajaxFX.TriggerCollidingFX(PlayerData.Stats.RecoverCooldown, side);
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
        private void OnTriggerRay()
        {
            Vector2 force = Vector2.right * FacingValue * playerData.Projectile.Speed;
            RayProjectile projectile = Instantiate(playerData.Projectile.Projectile, playerData.Projectile.Origin.position, Quaternion.identity);
            projectile.SetForce(force);
            Disposable.Bind(projectile.gameObject, playerData.Projectile.Lifetime);
        }

    }
}

