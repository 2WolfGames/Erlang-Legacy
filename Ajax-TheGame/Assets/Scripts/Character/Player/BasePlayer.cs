using UnityEngine;
using Core.Shared;
using Core.Shared.Enum;
using Core.Character.Player.Ability;
using Core.Character.Player.Util;
using Core.Combat.Projectile;
using Core.Util;

namespace Core.Character.Player
{
    public class BasePlayer : BaseCharacter
    {
        [Header("Dash ability")]
        [SerializeField] Dash dashAttack;

        [Header("Ray ability")]
        [SerializeField] RayProjectile rayPrefab;
        [SerializeField] float raySpeed = 10f;
        [SerializeField] float rayLifetime = 10f;

        [Header("Configurations")]
        [SerializeField] float recoverTime = 1.5f;

        Collider2D ajaxCollider;
        MovementController ajaxMovement;
        FXController ajaxFX;
        Protectable playerProtection;
        Orientation ajaxOrientation;
        AbilityController abilityController;
        static BasePlayer instance;

        // Ajax can not move & can not fire any hability
        // freeze state change when Ajax was hit by some enemy
        // trigger by hit1 & hit2 animation by now
        bool freeze = false;

        public bool Freeze => freeze;

        public bool CanBeHit => playerProtection.CanBeHit;

        public static BasePlayer Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<BasePlayer>();
                return instance;
            }
        }

        protected override void OnAwake()
        {
            ajaxMovement = GetComponent<MovementController>();
            ajaxFX = GetComponent<FXController>();
            playerProtection = GetComponent<Protectable>();
            ajaxOrientation = GetComponent<Orientation>();
            abilityController = GetComponent<AbilityController>();
            ajaxCollider = GetComponent<Collider2D>();
        }

        // pre: triggered on hit over player animation
        // post: change Ajax freeze state
        // desc: while freeze is setted to true, some Ajax features should be detached
        //      Ajax should not be able to move, or trigger it's habilities
        //      this, method are called at start and end of hit animations
        public void SetFreeze(bool freeze)
        {
            this.freeze = freeze;

            if (this.freeze)
            {
                // detach scripts
                ajaxMovement.Freeze();
                ajaxMovement.enabled = false;
                ajaxOrientation.enabled = false;
                abilityController.enabled = false;
            }
            else
            {
                // attach scripts
                ajaxMovement.enabled = true;
                ajaxOrientation.enabled = true;
                abilityController.enabled = true;
            }
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
            playerProtection.ResetProtection(); // can not interact with game object for a while
            Side side = Function.CollisionSide(transform, other.transform);
            ajaxFX.TriggerCollidingFX(recoverTime, side);
            TakeLife(damage); // takes one life
        }

        public void Dash(float dashTime)
        {
            StartCoroutine(ajaxFX.InhibitFlip(dashTime));
            StartCoroutine(ajaxFX.DashCoroutine(dashTime));
            StartCoroutine(ajaxMovement.DashCoroutine(FacingTo(), dashTime));
            StartCoroutine(dashAttack.AttackCoroutine(dashTime));
            playerProtection.ResetProtection(dashTime);
        }

        // pre: --
        // post: instanciate a ray prefab that will destroy itself in n seconds
        public void Ray(Vector3 origin)
        {
            bool left = FacingTo() == PlayerFacing.Left;
            var orientation = left ? -1f : 1f;
            Vector2 force = Vector2.right * orientation * raySpeed;
            RayProjectile projectile = Instantiate(rayPrefab, origin, Quaternion.identity);
            projectile.SetForce(force);
            projectile.OnProjectileCollided += (Collider2D collider) => Debug.Log(collider); // TODO: add correct collide function
            Disposable.Bind(projectile.gameObject, rayLifetime);
        }

        // pre: --
        // post: remove other animations & goes to idle animation
        public void Idle()
        {
            // may to implement later?
        }

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

        // pre: --
        // returns: Ajax's collider
        public Collider2D GetCollider()
        {
            return ajaxCollider;
        }

        // -1 | 0 | 1
        public int HorizontalInputNormalized()
        {
            return ajaxOrientation.InputToNumber();
        }

        public PlayerFacing FacingTo()
        {
            return ajaxOrientation.LatestFacing;
        }

    }
}

