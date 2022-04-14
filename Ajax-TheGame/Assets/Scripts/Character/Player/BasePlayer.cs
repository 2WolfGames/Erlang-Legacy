using UnityEngine;
using Core.Shared;
using Core.Shared.Enum;
using Core.Combat.Projectile;
using Core.Util;
using Core.Combat;

// todo: rename this file to PlayerManager & removes base character herarchy
namespace Core.Character.Player
{
    public class BasePlayer : BaseCharacter
    {
        [SerializeField] PlayerData playerData;
        [SerializeField] RayProjectile rayProjectile;
        [SerializeField] HitArea dashHitArea;
        [SerializeField] HitArea punchHitArea; // basic attack damage area
        private MovementController ajaxMovement;
        private FXController ajaxFX;
        private Protectable playerProtection;
        private PlayerFacingManager playerFacingManager;
        private PlayerAbilitiesManager playerAbilitiesManager;
        private bool controllable = true;
        private bool blockingUI;
        public bool CanBeHit => playerProtection.CanBeHit;
        public bool IsDashing => playerAbilitiesManager.IsDashing;
        public PlayerFacing Facing => playerFacingManager.Facing;
        public int FacingValue => playerFacingManager.FacingToInt;
        public PlayerData PlayerData => playerData;
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
        public Animator Animator => GetComponent<Animator>();
        public static BasePlayer Instance;

        protected override void OnAwake()
        {
            // global game instance
            Instance = this;

            ajaxMovement = GetComponent<MovementController>();
            ajaxFX = GetComponent<FXController>();
            playerFacingManager = GetComponent<PlayerFacingManager>();

            playerProtection = GetComponent<Protectable>();
            playerProtection.ProtectionDuration = PlayerData.recoverCooldown;

            playerAbilitiesManager = GetComponent<PlayerAbilitiesManager>();
            playerAbilitiesManager.OnTriggerDash += OnTriggerDash;
            playerAbilitiesManager.OnTriggerRay += OnTriggerRay;

            dashHitArea.OnHit += OnDashHit;

            rayProjectile.OnHit += OnRayProjectileHit;
        }

        // pre: called by some function that stunds player (called by hit animation)
        // post: enable scripts & returns normal game constants
        private void OnControllable()
        {
            ajaxMovement.enabled = true;
            playerFacingManager.enabled = true;
            playerAbilitiesManager.enabled = true;
            // reset global game constant to normal state if needed...
        }

        // pre: (called by hit end animation)
        // post: detach component logic scripts & freeze player
        // PROP: instead of freezing player we can make time slow 
        private void OnUncontrollable()
        {
            ajaxMovement.Freeze();
            ajaxMovement.enabled = false;
            playerFacingManager.enabled = false;
            playerAbilitiesManager.enabled = false;
            // set momentanious game constants if needed...
        }

        // pre: --
        // post: applies damage to player
        public void OnCollision(GameObject other, int damage = 1)
        {
            if (playerProtection.IsProtected) return;
            Hurt(damage, other);
        }

        // pre: --
        // post: applies damage to player
        public override void Hurt(int damage, GameObject other)
        {
            if (playerProtection.IsProtected) return;
            playerProtection.ResetProtection(PlayerData.recoverCooldown);
            Side side = Function.CollisionSide(transform, other.transform);
            ajaxFX.TriggerCollidingFX(PlayerData.recoverCooldown, side);
            TakeLife(damage); // takes one life
        }

        private void OnTriggerDash()
        {
            Debug.Log("Trigger dash");
            // StartCoroutine(ajaxFX.InhibitFlip(playerData.dashDuration));
            StartCoroutine(ajaxFX.DashCoroutine(playerData.dashDuration));
            StartCoroutine(ajaxMovement.DashCoroutine(Facing, playerData.dashDuration));
            StartCoroutine(dashHitArea.Hit(playerData.dashDuration));
            playerProtection.ResetProtection(playerData.dashDuration);
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
            Vector2 force = Vector2.right * FacingValue * playerData.raySpeed;
            RayProjectile projectile = Instantiate(rayProjectile, playerData.rayOrigin.position, Quaternion.identity);
            projectile.SetForce(force);
            Disposable.Bind(projectile.gameObject, playerData.rayLifetime);
        }

        // todo: make this functions privates & pass to PlayerMovementManager at script awake
        public void Run(bool run)
        {
            ajaxFX.SetRunFX(run);
        }

        public void Land()
        {
            ajaxFX.TriggerLandFX();
        }

        public void Jump()
        {
            ajaxFX.TriggerJumpFX();
        }

    }
}

