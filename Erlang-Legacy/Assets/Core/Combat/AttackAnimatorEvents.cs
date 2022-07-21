using Core.Manager;
using UnityEngine;

namespace Core.Combat
{
    public class AttackAnimatorEvents : MonoBehaviour
    {
        public Collider2D attackCollider;
        public ParticleSystem impactEffect;
        public Transform impactTransform;
        public float cameraShakeIntensity = 0.2f;

        private void OnAttackStart()
        {
            attackCollider.enabled = true;
            EffectManager.Instance?.PlayOneShot(impactEffect, impactTransform);
            CameraManager.Instance?.ShakeCamera(cameraShakeIntensity);
        }

        private void OnAttackEnd()
        {
            attackCollider.enabled = false;
        }
    }
}
