using Core.Combat;
using Core.Combat.Projectile;
using Core.Player.Data;
using Core.Player.Utility;
using Core.Shared;
using Core.Shared.Enum;
using Core.Utility;
using UnityEngine;
using Core.UI;

namespace Core.Player.Controller
{
    public class AbilityController : MonoBehaviour
    {
        [Range(0.1f, 1f)]
        public float punchDrag = 0.2f;
        public float punchMemoryDuration = 2f;
        public ProjectileData projectileData;
        public DamageAreaData damageAreas;
        public ParticleSystem punchParticle;
        public ParticleSystem dashParticle;
        public AbilitiesAcquired abilitiesAcquired;
        public bool Punching => punching;
        private enum Fist { L, R }
        private InteractOnTrigger2D dashTrigger => damageAreas.Dash;
        private InteractOnTrigger2D punchTrigger => damageAreas.Punch;
        private float rayTimer;
        private Fist fist;
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

            if (Input.GetButton(CharacterActions.InvokeRay) && CanInvokeRayAbility())
                InvokeRayAbility();
        }

        public void FixedUpdate()
        {
            if (CanPunch())
                PunchStart();
            if (wannaPunch) wannaPunch = false;
        }

        public void PunchEnd()
        {
            if (!punching)
                return;
            punching = false;
            punchMemoryTimer = punchMemoryDuration;
            punchTrigger.Interact = false;
            movementController.Acceleration = 1f;
        }

        public bool AdquiredAbility(Ability ability)
        {
            if (abilitiesAcquired == null)
            {
                Debug.LogError("Please make sure to add abilities scriptable object manager");
                return false;
            }
            return abilitiesAcquired.Acquired(ability);
        }

        public void AdquireAbility(Ability ability)
        {
            if (abilitiesAcquired == null)
            {
                Debug.LogError("Please make sure to add abilities scriptable object manager");
            }
            else
            {
                abilitiesAcquired.Acquire(ability);
            }
        }

        private bool CanPunch()
        {
            return wannaPunch && controllable;
        }

        private void PunchStart()
        {
            if (punching)
                return;
            punching = true;
            fist = ForgotNextFist() ? RandomFist() : NextFist();
            punchTrigger.Interact = true;
            punching = true;
            movementController.Acceleration = punchDrag;
            AnimatePunch(fist);
            punchParticle?.Play();
        }

        private bool ForgotNextFist()
        {
            return punchMemoryTimer <= 0;
        }

        private void AnimatePunch(Fist fist)
        {
            if (fist == Fist.L)
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
            return fist == Fist.L ? Fist.R : Fist.L;
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
            if (dashParticle)
                dashParticle.Play();
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

        private void InvokeRayAbility()
        {
            animator.SetTrigger(CharacterAnimations.Ray);
            ResetRayCooldown();
            RayAbilityStart();
        }

        private void RayAbilityStart()
        {
            player.Controllable = false;
            player.Freeze();
            player.ZeroGravity();
        }

        // called at end of ray animation as event
        public void RayAbilityComplete()
        {
            player.Controllable = true;
        }

        // called by ray player animation as event
        public void InvokeRayBallInstance()
        {
            Vector2 force = Vector2.right * FacingValue * projectileSpeed;
            VengefulProjectile instance = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity);
            instance.SetForce(force);
            instance.gameObject.Disposable(projectileTimeout);

            player.BaseGravity();
        }

        private void ResetRayCooldown()
        {
            rayTimer = rayCooldown;
            PowersPanelManager.Instance?.GetRayTimer().PowerUsed(rayCooldown);
        }

        private bool CanInvokeRayAbility()
        {
            return rayTimer <= 0 && controllable && AdquiredAbility(Ability.Ray);
        }

        public void OnRayHit(Collider2D other)
        {
            //This is a Unity Event
            OnHit(other, playerStats.rayDamage);
        }

    }
}
