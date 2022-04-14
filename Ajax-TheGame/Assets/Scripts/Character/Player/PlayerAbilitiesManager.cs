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
        private float RayCooldown => BasePlayer.Instance.PlayerData.rayCooldown;
        private float DashCooldown => BasePlayer.Instance.PlayerData.dashCooldown;
        private float DashDuration => BasePlayer.Instance.PlayerData.dashDuration;
        public bool CanTriggerDash => dashTimer <= 0 && !IsDashing;
        public bool CanTriggerRay => rayTimer <= 0 && !IsDashing;
        public bool IsDashing { get; private set; }
        public Action OnTriggerDash;
        public Action OnTriggerRay;

        public void Update()
        {
            if (rayTimer > 0)
                rayTimer -= Time.deltaTime;
            if (dashTimer > 0)
                dashTimer -= Time.deltaTime;

            if (Input.GetButtonDown("Fire1") && CanTriggerDash)
                TriggerDash();
            if (Input.GetButton("Fire2") && CanTriggerRay)
                TriggerRay();
        }

        private void TriggerDash()
        {
            IsDashing = true;
            OnTriggerDash?.Invoke();
            ResetDashTimer();
            StartCoroutine(ResetIsDashing());
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

        private void ResetDashTimer()
        {
            BasePlayer player = BasePlayer.Instance;
            dashTimer = DashCooldown;
        }

        private IEnumerator ResetIsDashing()
        {
            BasePlayer player = BasePlayer.Instance;
            yield return new WaitForSeconds(DashDuration);
            IsDashing = false;
        }
    }

}