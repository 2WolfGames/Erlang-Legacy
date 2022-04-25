using UnityEngine;
using Core.Player.Data;
using Core.Combat.Projectile;
using Core.Util;
using Core.Combat;

namespace Core.Player.Controller
{
    // description:
    //   manages when abilities can be triggered
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] ProjectileData projectile;
        [SerializeField] DamageAreaData damageAreas;

        private Triggerable Dash => damageAreas.Dash;
        private ParticleSystem dashPS => damageAreas.DashParticle;
        private Triggerable Punch => damageAreas.Punch;

        private float rayTimer;
        private PlayerController Player => PlayerController.Instance;
        private PlayerData PlayerData => Player.PlayerData;
        private float RayCooldown => PlayerData.Stats.RayCooldown;
        private int FacingValue => Player.FacingValue;
        public bool CanInvokeRay => rayTimer <= 0 && Player.Controllable;

        public void Awake()
        {
            Dash.Enabled = false;
            Dash.OnEnter += OnDashHitEnters;
            Punch.Enabled = false;
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
                InvokeRay();
        }

        // TODO: work with hittable objects too
        private void OnDashHitEnters(Collider2D other)
        {
            var destroyable = other.GetComponent<Destroyable>();

            if (destroyable)
            {
                destroyable.OnDestroyed += () => Debug.Log($"enemy is dead {this.gameObject.name}");
                var direction = Player.transform.position.x > other.transform.position.x ? -1 : 1;
                destroyable.OnAttackHit(Vector2.right * direction, 1); // TODO: set default hit damage
            }
        }

        // pre: --
        // post: active dash damage area
        public void ActiveDashDamage()
        {
            Dash.Enabled = true;
            if (dashPS)
                EffectManager.Instance.PlayOneShot(dashPS, dashPS.transform.position);
        }

        // pre: --
        // post: deactive dash damage area
        public void DeactiveDashDamage()
        {
            Dash.Enabled = false;
        }

        private void OnThrowPunch()
        {
            Debug.Log("punching");
            Punch.Enabled = true;
        }

        private void OnPickUpPunch()
        {
            Debug.Log("pickup punch");
            Punch.Enabled = false;
        }

        // pre: --
        // post: invoke an instance of ray projectile and sets its values
        private void InvokeRay()
        {
            Vector2 force = Vector2.right * FacingValue * this.projectile.Speed;
            RayProjectile instance = Instantiate(projectile.Projectile, projectile.Origin.position, Quaternion.identity);
            instance.SetForce(force);
            instance.OnColliding += OnRayColliding;
            Disposable.Bind(instance.gameObject, projectile.Lifetime);
            rayTimer = RayCooldown;
        }

        private void OnRayColliding(Collider2D other)
        {
            // make enemy damage
            Debug.Log("Hitting enemy at ray");
            OnHit(other, PlayerData.Stats.RayDamage);
        }

        private void OnHit(Collider2D other, float damage)
        {

        }




    }

}