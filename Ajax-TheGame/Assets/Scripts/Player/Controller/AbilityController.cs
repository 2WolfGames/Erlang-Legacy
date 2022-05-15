using Core.Combat;
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

        private InteractOnTrigger2D Dash => damageAreas.Dash;
        private InteractOnTrigger2D Punch => damageAreas.Punch;
        private float rayTimer;
        private PlayerController Player => PlayerController.Instance;
        private PlayerData PlayerData => Player.PlayerData;
        private RayProjectile projectilePrefab => projectileData.Projectile;
        private float projectileSpeed => projectileData.Speed;
        private float projectileTimeout => projectileData.Lifetime;
        private Transform projectileOrigin => projectileData.Origin;
        private float RayCooldown => PlayerData.Stats.rayCooldown;
        private int FacingValue => Player.FacingValue;
        private Animator animator => Player.Animator;
        public bool CanInvokeRay => rayTimer <= 0 && Player.Controllable;

        public void Awake()
        {
            Dash.enabled = false;
            Punch.enabled = false;
        }

        public void Update()
        {
            if (rayTimer > 0)
                rayTimer -= Time.deltaTime;

            if (Input.GetButtonDown("Punch"))
                OnThrowPunch();

            if (Input.GetButtonUp("Punch"))
                OnPickUpPunch();

            if (Input.GetButton("Ray") && CanInvokeRay)
                OnRayAnimationStart();
        }

        public void ActiveDashDamage()
        {
            if (punchParticle)
                punchParticle.Play();
            Dash.enabled = true;
        }

        public void DeactiveDashDamage()
        {
            Dash.enabled = false;
        }

        private void OnThrowPunch()
        {
            Debug.Log("punching");
            Punch.enabled = true;
        }

        private void OnPickUpPunch()
        {
            Debug.Log("pickup punch");
            Punch.enabled = false;
        }

        private void OnRayAnimationStart()
        {
            animator.SetTrigger(CharacterAnimations.Ray);
            rayTimer = RayCooldown;
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
            OnHit(other, PlayerData.Stats.rayDamage);
        }

        private void OnHit(Collider2D other, float damage)
        {

        }
    }

}
