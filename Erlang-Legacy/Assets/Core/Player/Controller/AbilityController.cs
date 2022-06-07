using Core.Combat;
using Core.Combat.Projectile;
using Core.Player.Data;
using Core.Player.Util;
using Core.Shared;
using Core.Shared.Enum;
using Core.Utility;
using DG.Tweening;
using UnityEngine;


namespace Core.Player.Controller
{
    // description:
    //   manages when abilities can be triggered
    public class AbilityController : MonoBehaviour
    {
        private enum Fist
        {
            L, R
        }

        [Range(0.1f, 1f)] public float punchDrag = 0.2f;
        public float punchMemoryDuration = 2f;
        [SerializeField] ProjectileData projectileData;
        [SerializeField] DamageAreaData damageAreas;
        [SerializeField] ParticleSystem punchParticle;
        public bool Punching => punching;
        private InteractOnTrigger2D dashTrigger => damageAreas.Dash;
        private InteractOnTrigger2D punchTrigger => damageAreas.Punch;
        private float rayTimer;
        private Fist punchFist;
        private bool punching;
        private float punchMemoryTimer;
        private PlayerController player => PlayerController.Instance;
        private PlayerData PlayerData => player.PlayerData;
        private VengefulProjectile projectilePrefab => projectileData.Projectile;
        private float projectileSpeed => projectileData.Speed;
        private float projectileTimeout => projectileData.Lifetime;
        private Transform projectileOrigin => projectileData.Origin;
        private float rayCooldown => PlayerData.Stats.rayCooldown;
        private int FacingValue => player.FacingValue;
        private Animator animator => player.Animator;
        public bool CanInvokeRay => rayTimer <= 0 && controllable;
        private bool wannaPunch = false;
        private bool controllable => player.Controllable;
        private Stats playerStats => player.Stats;
        private MovementController movementController => GetComponent<MovementController>();

        public void Start()
        {
            dashTrigger.Interact = false;
            punchTrigger.Interact = false;
        }

        public void Update()
        {
            if (rayTimer > 0)
                rayTimer -= Time.deltaTime;

            if (punchMemoryTimer > 0)
                punchMemoryTimer -= Time.deltaTime;

            if (Input.GetButtonDown(CharacterActions.Punch))
            {
                wannaPunch = true;
            }

            if (Input.GetButton(CharacterActions.InvokeRay) && CanInvokeRay)
                OnRayAnimationStart();
        }

        public void FixedUpdate()
        {
            if (wannaPunch && controllable)
            {
                Punch();
            }

            if (wannaPunch) wannaPunch = false;
        }

        private void Punch()
        {
            if (punching)
                return;

            PunchStart();
            DOVirtual.DelayedCall(0.1f, PunchEnd);
        }

        private void PunchStart()
        {
            if (punchMemoryTimer <= 0)
            {
                punchFist = RandomFist();
            }
            else
            {
                punchFist = NextFist();
            }
            punchTrigger.Interact = true;
            punching = true;
            punchParticle?.Play();
            movementController.Acceleration = punchDrag;
            StartPunchAnimation(punchFist);
        }

        private void PunchEnd()
        {
            punchMemoryTimer = punchMemoryDuration;
            punchTrigger.Interact = false;
            punching = false;
            movementController.Acceleration = 1f;
        }

        private void StartPunchAnimation(Fist punchFist)
        {
            if (punchFist == Fist.L)
            {
                animator.SetTrigger(CharacterAnimations.LPunch);
            }
            else
            {
                animator.SetTrigger(CharacterAnimations.RPunch);
            }
        }

        private Fist NextFist()
        {
            if (punchFist == Fist.L)
                return Fist.R;
            else
                return Fist.L;
        }

        private Fist RandomFist()
        {
            return (Fist)Random.Range(0, 2);
        }

        public void OnDashComplete()
        {
            dashTrigger.Interact = false;
        }

        public void OnDashStart()
        {
            ActiveDashDamage();
        }

        private void ActiveDashDamage()
        {
            if (punchParticle)
                punchParticle.Play();

            dashTrigger.Interact = true;
        }


        public void OnPunchLand(Collider2D other)
        {
            int damage = playerStats.punchDamage;
            OnHit(other, damage);
        }

        public void OnSpearLand(Collider2D other)
        {
            int damage = playerStats.dashDamage;
            OnHit(other, damage);
        }

        private void OnHit(Collider2D other, int damage)
        {
            Destroyable destroyable = other.GetComponent<Destroyable>();

            Face face = Function.CollisionSide(other.transform, transform);
            Vector2 direction = face == Face.Right ? Vector2.right : Vector2.left;

            destroyable?.OnAttackHit(damage, direction);
        }

        private void OnRayAnimationStart()
        {
            animator.SetTrigger(CharacterAnimations.Ray);
            ResetRayCooldown();
        }

        private void ResetRayCooldown()
        {
            rayTimer = rayCooldown;
        }

        // pre: called by ray player animation
        // post: invoke an instance of ray projectile and sets its values
        public void InvokeRay()
        {
            Vector2 force = Vector2.right * FacingValue * projectileSpeed;
            VengefulProjectile instance = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity);
            instance.SetForce(force);
            instance.gameObject.Disposable(projectileTimeout);
        }

        public void OnRayHit(Collider2D other)
        {
            Debug.Log("ray hit");
            OnHit(other, playerStats.rayDamage);
        }
    }

}
