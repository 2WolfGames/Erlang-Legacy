using System.Collections;
using UnityEngine;
using System;


namespace Core.Character.Player
{
    // description:
    //   manages when abilities can be triggered
    public class PlayerAbilitiesManager : MonoBehaviour
    {
        private float rayTimer;
        private float dashTimer;
        private float RayCooldown => BasePlayer.Instance.PlayerData.Stats.RayCooldown;
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
            BasePlayer player = BasePlayer.Instance;
            rayTimer = RayCooldown;
        }

    }

}