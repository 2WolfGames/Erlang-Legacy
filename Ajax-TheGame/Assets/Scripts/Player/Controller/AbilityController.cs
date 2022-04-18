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
        private float rayTimer;
        private PlayerController Player => PlayerController.Instance;
        PlayerData PlayerData => Player.PlayerData;
        private float RayCooldown => PlayerData.Stats.RayCooldown;
        private int FacingValue => Player.FacingValue;
        public bool CanTriggerRay => rayTimer <= 0 && Player.Controllable;
        public Action OnRayStart { get; set; }

        public void Update()
        {
            if (rayTimer > 0)
                rayTimer -= Time.deltaTime;

            if (Input.GetButtonDown("Punch"))
            {
                Debug.Log("punching");
            }

            if (Input.GetButtonUp("Punch"))
            {
                Debug.Log("ends punch");
            }

            if (Input.GetButton("Ray") && CanTriggerRay)
                TriggerRay();
        }


        private void TriggerRay()
        {
            OnRayStart?.Invoke();
            Vector2 force = Vector2.right * FacingValue * PlayerData.Projectile.Speed;
            RayProjectile projectile = Instantiate(PlayerData.Projectile.Projectile, PlayerData.Projectile.Origin.position, Quaternion.identity);
            projectile.SetForce(force);
            Disposable.Bind(projectile.gameObject, PlayerData.Projectile.Lifetime);
            rayTimer = RayCooldown;
        }
    }

}