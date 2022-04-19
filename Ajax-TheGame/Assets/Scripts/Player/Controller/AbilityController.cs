using UnityEngine;
using System;
using Core.Player.Data;
using Core.Combat.Projectile;
using Core.Util;

namespace Core.Player.Controller
{
    // description:
    //   manages when abilities can be triggered
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] ProjectileData projectile;
        [SerializeField] DamageAreaData damageAreas;

        private float rayTimer;
        private PlayerController Player => PlayerController.Instance;
        PlayerData PlayerData => Player.PlayerData;
        private float RayCooldown => PlayerData.Stats.RayCooldown;
        private int FacingValue => Player.FacingValue;
        public bool CanInvokeRay => rayTimer <= 0 && Player.Controllable;
        public Action OnRayStart { get; set; }

        public void Awake()
        {
            damageAreas.Dash.SetEnabled(false);
            damageAreas.Punch.SetEnabled(false);
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

        private void OnThrowPunch()
        {
            Debug.Log("punching");
            damageAreas.Punch.SetEnabled(true);
        }

        private void OnPickUpPunch()
        {
            Debug.Log("pickup punch");
            damageAreas.Punch.SetEnabled(false);
        }


        // pre: --
        // post: invoke an instance of ray projectile and sets its values
        private void InvokeRay()
        {
            OnRayStart?.Invoke();
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


        public void OnTriggerEnter2D(Collider2D other)
        {
            // trigger because of punch hit or dash hit
        }


        private void OnHit(Collider2D other, float damage)
        {

        }




    }

}