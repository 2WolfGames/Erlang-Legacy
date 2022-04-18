using System.Collections;
using UnityEngine;
using System;


namespace Core.Player.Controller
{
    // description:
    //   manages when abilities can be triggered
    public class AbilityController : MonoBehaviour
    {
        private float rayTimer;
        private float dashTimer;
        private float RayCooldown => PlayerController.Instance.PlayerData.Stats.RayCooldown;
        public bool CanTriggerDash => dashTimer <= 0 && !IsDashing;
        public bool CanTriggerRay => rayTimer <= 0 && !IsDashing;
        public bool IsDashing { get; private set; }
        public Action OnTriggerRay;

        public void Update()
        {
            if (rayTimer > 0)
                rayTimer -= Time.deltaTime;
            if (dashTimer > 0)
                dashTimer -= Time.deltaTime;

            if (Input.GetButton("Fire2") && CanTriggerRay)
                TriggerRay();
        }


        private void TriggerRay()
        {
            OnTriggerRay?.Invoke();
            ResetRayTimer();
        }

        private void ResetRayTimer()
        {
            PlayerController player = PlayerController.Instance;
            rayTimer = RayCooldown;
        }

    }

}