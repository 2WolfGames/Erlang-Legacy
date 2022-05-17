using Core.Combat.Projectile;
using Core.Player.Data;
using Core.Player.Util;
using Core.Utility;
using DG.Tweening;
using UnityEngine;


namespace Core.Player.Controller
{
    // description:
    //   manages when abilities can be triggered
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] ProjectileData projectileData;
        [SerializeField] DamageAreaData damageAreas;
        [SerializeField] ParticleSystem punchParticle;

        private InteractOnTrigger2D dashTrigger => damageAreas.Dash;
        private InteractOnTrigger2D punchTrigger => damageAreas.Punch;
        private float rayTimer;
        private PlayerController Player => PlayerController.Instance;
        private PlayerData PlayerData => Player.PlayerData;
        private RayProjectile projectilePrefab => projectileData.Projectile;
        private float projectileSpeed => projectileData.Speed;
        private float projectileTimeout => projectileData.Lifetime;
        private Transform projectileOrigin => projectileData.Origin;
        private float rayCooldown => PlayerData.Stats.rayCooldown;
        private int FacingValue => Player.FacingValue;
        private Animator animator => Player.Animator;
        public bool CanInvokeRay => rayTimer <= 0 && controllable;
        private bool wannaPunch = false;
        private bool punching = false;
        private bool controllable => Player.Controllable;
        private bool shouldFlurryPunches => wannaPunch && !punching && controllable;

        public void Awake()
        {
            dashTrigger.Interact = false;
            punchTrigger.Interact = false;
        }

        public void Update()
        {
            if (rayTimer > 0)
                rayTimer -= Time.deltaTime;

            if (Input.GetButtonDown(CharacterActions.Punch))
            {
                wannaPunch = true;
            }

            if (Input.GetButtonUp(CharacterActions.Punch))
            {
                wannaPunch = false;
            }

            if (Input.GetButton(CharacterActions.InvokeRay) && CanInvokeRay)
                OnRayAnimationStart();
        }

        public void FixedUpdate()
        {
            FlurryPuches();
        }

        public void ActiveDashDamage()
        {
            if (punchParticle)
                punchParticle.Play();
            dashTrigger.Interact = true;
        }

        public void DeactiveDashDamage()
        {
            dashTrigger.Interact = false;
        }

        private void FlurryPuches()
        {
            if (!shouldFlurryPunches)
                return;

            punchTrigger.Interact = true;
            punching = true;

            animator.SetTrigger(CharacterAnimations.FlurryPunches);

            DOVirtual.DelayedCall(1f, PickUpPunch);
        }

        private void PickUpPunch()
        {
            punchTrigger.Interact = false;
            punching = false;
        }

        public void OnPunchLand(Collider2D other)
        {
            Debug.Log("Punch land in enemy's face");
            Debug.Log(other.name);
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
            RayProjectile instance = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity);
            instance.SetForce(force);
            instance.OnColliding += OnRayColliding;
            instance.gameObject.Disposable(projectileTimeout);
        }

        private void OnRayColliding(Collider2D other)
        {
            // make enemy damage
            Debug.Log("Hitting enemy at ray");
        }

    }

}
