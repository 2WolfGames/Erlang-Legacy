using System;
using System.Collections.Generic;
using Core.Shared;
using Core.Utility;
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
        [SerializeField] bool hitShakeCamera;
        [SerializeField] float recoilForce;
        protected Vector2 baseScale;
        protected Color baseColor;
        protected Material baseMaterial;
        private Color defaultColor = Color.white;
        private Rigidbody2D body => GetComponent<Rigidbody2D>();
        public Action OnHit;

        List<Tween> tweens = new List<Tween>();

        // Start is called before the first frame update
        public virtual void Awake()
        {
            Init();
        }

        public void OnAttackHit(Vector2 attackHitDirection)
        {
            OnHit?.Invoke();

            Tween tween = null;

            if (hitType == HitType.Inflate)
            {
                tween = DOTween.Sequence();
                (tween as Sequence)
                    .Append(transform.DOScale(baseScale * 0.9f, 0.1f))
                    .Append(transform.DOScale(baseScale, 0.1f))
                    .SetEase(Ease.Linear);
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
                tween = DOTween.Sequence();
                (tween as Sequence)
                    .Append(sprite.DOColor(hitColor, 0.1f))
                    .Append(sprite.DOColor(baseColor, 0.1f));
            }
            else if (hitType == HitType.Material)
            {
                sprite.material = hitMaterial;
                tween = DOVirtual.DelayedCall(0.25f, () => sprite.material = baseMaterial);
            }
            else if (hitType == HitType.Animation)
            {
                animator.Play(hitAnimation.name);
            }

            // add new tween to list
            if (tween != null)
                tweens.Add(tween);

            if (customHitEffect != null)
                EffectManager.Instance?.PlayOneShot(customHitEffect, transform.position);

            if (customHitSound != null)
                SoundManager.Instance?.PlaySoundAtLocation(customHitSound, transform.position);

            if (hitShakeCamera)
            {
                CameraManager.Instance.ShakeCamera();
            }
        }

        public void OnDestroy()
        {
            Function.KillTweensThatStillAlive(tweens);
        }

        private void Init()
        {
            EvaluateConfiguration();
            InitializeAttributes();
        }

        private void EvaluateConfiguration()
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
        }

        private void InitializeAttributes()
        {
            if (sprite)
            {
                baseColor = sprite.color;
                baseMaterial = sprite.material;
            }
            else
            {
                baseColor = Color.white;
            }
            baseScale = transform.localScale;
        }
    }
}
