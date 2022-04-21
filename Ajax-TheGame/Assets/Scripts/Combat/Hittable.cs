using System;
using Core.Util;
using DG.Tweening;
using UnityEngine;

namespace Core.Combat
{
    public class Hittable : MonoBehaviour
    {
        public enum HitType
        {
            None,
            Inflate, // scale
            Push,
            Color,
            Material,
            Animation
        }
        [SerializeField] HitType hitType = HitType.None;
        [SerializeField] ParticleSystem customHitEffect;
        [SerializeField] AudioClip customHitSound;
        [SerializeField] Color hitColor = new Color(0.2f, 0.0f, 0.0f);
        [SerializeField] Material hitMaterial;
        [SerializeField] Animation hitAnimation;
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] bool shakeCamera;
        protected Vector2 baseScale;
        protected Color baseColor;
        protected Material baseMaterial;
        private Color defaultColor = Color.white;

        public Action<Vector2, Vector2> OnHit;

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            if (hitType == HitType.Animation)
            {
                if (animator == null)
                {
                    Debug.LogError("Hit type declared as Animation but no animator provided");
                    return;
                }

                if (hitAnimation == null)
                {
                    Debug.LogError("Hit type declared as Animation but no animation provided");
                    return;

                }
            }

            if (hitType == HitType.Color && sprite == null)
            {
                Debug.LogError("Hit type declared as Color but no sprite renderer provided");
                return;
            }

            if (hitType == HitType.Material && sprite == null)
            {
                Debug.LogError("Hit type declared as Material but no sprite renderer provided");
                return;
            }

            baseColor = sprite == null ? defaultColor : sprite.color;
            baseMaterial = sprite?.material;
            baseScale = transform.localScale;
        }

        public virtual void OnAttackHit(Vector2 position, Vector2 force, int damage)
        {
            // Hurt/Damage
            OnHit?.Invoke(position, force);

            if (hitType == HitType.Inflate)
            {
                var sequence = DOTween.Sequence();
                sequence
                    .Append(transform.DOScale(baseScale * 0.1f, 0.25f))
                    .Append(transform.DOScale(baseScale, 0.25f))
                    .SetEase(Ease.InOutElastic);
            }
            else if (hitType == HitType.Push)
            {
                Debug.Log("push hit not supported yet");
                // var sequence = DOTween.Sequence();
                // sequence
                //     .Append(transform.DOMoveX()
                //     .Append(transform.DOScale(baseScale, 0.25f))
                //     .SetEase(Ease.InOutElastic);
            }
            else if (hitType == HitType.Color)
            {
                // change color, reset color in few seconds
                var sequence = DOTween.Sequence();
                sequence
                    .Append(sprite.DOColor(hitColor, 0.25f))
                    .Append(sprite.DOColor(baseColor, 0.25f));
            }
            else if (hitType == HitType.Material)
            {
                sprite.material = hitMaterial;
                DOVirtual.DelayedCall(0.25f, () => sprite.material = baseMaterial);
            }
            else if (hitType == HitType.Animation)
            {
                animator.Play(hitAnimation.name);
            }

            if (customHitEffect != null)
                EffectManager.Instance.PlayOneShot(customHitEffect, position);

            if (customHitSound != null)
                SoundManager.Instance.PlaySoundAtLocation(customHitSound, transform.position);

            if (shakeCamera)
            {
                // use camera controller to shake it
                Debug.Log("shaking camera...");
            }

        }

    }
}