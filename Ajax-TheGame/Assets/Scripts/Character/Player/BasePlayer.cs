using UnityEngine;
using Core.Shared;
using Core.Shared.Enum;
using Core.Combat.Projectile;
using Core.Util;
using Core.Util.Serializable;

// todo: rename this file to PlayerManager & removes base character herarchy
namespace Core.Character.Player
{
    public class BasePlayer : MonoBehaviour
    {
        [SerializeField] PlayerData playerData;
        private float baseGravityScale;
        private PlayerMovementManager playerMovementManager;
        private FXController ajaxFX;
        private Protectable playerProtection;
        private PlayerFacingManager playerFacingManager;
        private PlayerAbilitiesManager playerAbilitiesManager;
        private bool controllable = true;
        private bool blockingUI;
        public float BaseGravityScale => baseGravityScale;
        public bool CanBeHit => playerProtection.CanBeHit;
        public bool IsDashing => playerAbilitiesManager.IsDashing;
        public PlayerFacing Facing => playerFacingManager.Facing;
        public int FacingValue => playerFacingManager.FacingToInt;
        public PlayerData PlayerData => playerData;
        public bool IsGrounded => playerMovementManager.IsGrounded;
        public bool IsCornerTime => playerMovementManager.IsCornerTime;
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

        public static BasePlayer Instance { get; private set; }


        protected void Awake()
        {
            // global game instance

            var matches = FindObjectsOfType<BasePlayer>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;

            playerMovementManager = GetComponent<PlayerMovementManager>();
            ajaxFX = GetComponent<FXController>();
            playerFacingManager = GetComponent<PlayerFacingManager>();
            playerProtection = GetComponent<Protectable>();
            playerAbilitiesManager = GetComponent<PlayerAbilitiesManager>();


            playerAbilitiesManager.OnTriggerRay += OnTriggerRay;
            playerData.DamageArea.Dash.OnHit += OnDashHit;
            playerData.Projectile.Projectile.OnHit += OnRayProjectileHit;
            playerMovementManager.OnDashStart += OnDashStart;
            playerMovementManager.OnDashEnd += OnDashEnd;

            baseGravityScale = Body.gravityScale;
            playerProtection.ProtectionDuration = PlayerData.Stats.RecoverCooldown;
        }

        private void OnDashEnd()
        {
            playerFacingManager.enabled = true;
        }

        private void OnDashStart()
        {
            playerFacingManager.enabled = false;
        }

        // pre: called by some function that stunds player (called by hit animation)
        // post: enable scripts & returns normal game constants
        private void OnControllable()
        {
            Debug.Log("freeze game play");
            // playerFacingManager.enabled = true;
            // playerAbilitiesManager.enabled = true;
            // reset global game constant to normal state if needed...
        }

        // pre: (called by hit end animation)
        // post: detach component logic scripts & freeze player
        // PROP: instead of freezing player we can make time slow 
        private void OnUncontrollable()
        {
            Debug.Log("unfreeze game play");
            // playerFacingManager.enabled = false;
            // playerAbilitiesManager.enabled = false;
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
        public void Hurt(int damage, GameObject other)
        {
            if (playerProtection.IsProtected) return;
            playerProtection.ResetProtection(PlayerData.Stats.RecoverCooldown);
            Side side = Function.CollisionSide(transform, other.transform);
            ajaxFX.TriggerCollidingFX(PlayerData.Stats.RecoverCooldown, side);
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

        // todo: make this functions privates & pass to PlayerMovementManager at script awake
        public void Run(bool run)
        {
            ajaxFX.SetRunFX(run);
        }

        public void Land()
        {
            ajaxFX.TriggerLandFX();
        }


        public void OnDrawGizmosSelected()
        {
            Color rayColor = IsGrounded ? Color.green : Color.red;
            float extra = 0.1f;
            Debug.DrawRay(BodyCollider.bounds.center + new Vector3(BodyCollider.bounds.extents.x, 0), Vector2.down * (BodyCollider.bounds.extents.y + extra), rayColor);
            Debug.DrawRay(BodyCollider.bounds.center - new Vector3(BodyCollider.bounds.extents.x, 0), Vector2.down * (BodyCollider.bounds.extents.y + extra), rayColor);
            Debug.DrawRay(BodyCollider.bounds.center - new Vector3(BodyCollider.bounds.extents.x, BodyCollider.bounds.extents.y + extra), Vector2.right * (2 * BodyCollider.bounds.extents.x), rayColor);
        }
    }
}

