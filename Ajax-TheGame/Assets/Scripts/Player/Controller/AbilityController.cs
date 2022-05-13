using System;
using Core.Combat;
using Core.Combat.Projectile;
using Core.Player.Data;
using Core.Util;
using UnityEngine;

namespace Core.Player.Controller
{
    // description:
    //   manages when abilities can be triggered
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] ProjectileData projectile;
        [SerializeField] DamageAreaData damageAreas;

        private Triggerable Dash => damageAreas.Dash;
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
                OnInvokeRay();
        }

        // TODO: work with hittable objects too
        private void OnDashHitEnters(Collider2D other)
        {
            var destructable = other.GetComponent<Destructable>();

            if (destructable)
            {
                destructable.OnDestroyed += () => Debug.Log($"enemy is dead {this.gameObject.name}");
                var direction = Player.transform.position.x > other.transform.position.x ? -1 : 1;
                destructable.OnAttackHit(Vector2.right * direction, 1); // TODO: set default hit damage
            }
        }

        // pre: --
        // post: active dash damage area
        public void ActiveDashDamage()
        {
            Dash.Enabled = true;
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

        private void OnInvokeRay()
        {
            PlayerController.Instance.OnShootRay();
            rayTimer = RayCooldown;
        }

        // pre: --
        // post: invoke an instance of ray projectile and sets its values
        public void InvokeRay()
        {
            Vector2 force = Vector2.right * FacingValue * this.projectile.Speed;
            RayProjectile instance = Instantiate(projectile.Projectile, projectile.Origin.position, Quaternion.identity);
            instance.SetForce(force);
            instance.OnColliding += OnRayColliding;
            Disposable.Bind(instance.gameObject, projectile.Lifetime);
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
