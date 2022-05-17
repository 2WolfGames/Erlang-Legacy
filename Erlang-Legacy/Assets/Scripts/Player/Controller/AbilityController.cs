using Core.Combat.Projectile;
using Core.Player.Data;
using Core.Player.Util;
using Core.Utility;
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
        public bool CanInvokeRay => rayTimer <= 0 && Player.Controllable;

        private bool wannaPunch = false;
        private bool punching = false;
        private bool shouldPunch => wannaPunch && !punching;
        private bool shouldPickUpPunch => !wannaPunch;

        public void Awake()
        {
            dashTrigger.enabled = false;
            punchTrigger.enabled = false;
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
            if (shouldPunch)
            {
                Punch();
            }

            if (shouldPickUpPunch)
            {
                PickUpPunch();
            }
        }

        public void ActiveDashDamage()
        {
            if (punchParticle)
                punchParticle.Play();
            dashTrigger.enabled = true;
        }

        public void DeactiveDashDamage()
        {
            dashTrigger.enabled = false;
        }

        private void Punch()
        {
            Debug.Log("punching...");
            punchTrigger.enabled = true;
        }

        private void PickUpPunch()
        {
            Debug.Log("not punching...");
            punchTrigger.enabled = false;
        }

        public void OnPunchLand(Collider2D other)
        {
            Debug.Log("Punch land in enemy's face");
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
