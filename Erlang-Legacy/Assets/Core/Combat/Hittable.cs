using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Core.IA.Behavior;
using Core.Shared;
using Core.Manager;
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
            Sprite,
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
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Sprite hitSprite;
        [SerializeField] bool hitShakeCamera;
        public float recoilScale = 0f;
        protected Vector2 baseScale;
        protected Color baseColor;
        protected Material baseMaterial;
        private Color defaultColor = Color.white;
        private Rigidbody2D body => GetComponent<Rigidbody2D>();
        protected BehaviorTree behaviorTree => GetComponent<BehaviorTree>();
        private List<Tween> tweens = new List<Tween>();
        private Sprite baseSprite;

        // Start is called before the first frame update
        public virtual void Awake()
        {
            Init();
        }

        public void OnAttackHit()
        {
            ReactHit();
        }

        public void OnAttackHit(Vector2 direction)
        {
            ReactHit();
            Recoil(direction);
        }

        private void Recoil(Vector2 direction)
        {
            Vector2 norm = direction.normalized;
            if (behaviorTree)
                NotifyRecoilEvent(norm);
            else ApplyRecoil(norm);
        }

        private void ApplyRecoil(Vector2 direction)
        {
            Vector2 recoilForce = direction.normalized * recoilScale;
            body.AddForce(recoilForce, ForceMode2D.Impulse);
        }

        private void NotifyRecoilEvent(Vector2 direction)
        {
            behaviorTree.GetVariable(Variables.RecoilDirecton)?.SetValue(direction);
            behaviorTree.GetVariable(Variables.RecoilScale)?.SetValue(recoilScale);
            behaviorTree.SendEvent(Events.Recoil);
        }

        private void ReactHit()
        {
            Tween tween = null;

            if (hitType == HitType.Inflate)
            {
                tween = DOTween.Sequence();
                (tween as Sequence)
                    .Append(transform.DOScale(baseScale * 0.9f, 0.1f))
                    .Append(transform.DOScale(baseScale, 0.1f))
                    .SetEase(Ease.Linear);
            }
            else if (hitType == HitType.Sprite)
            {
                spriteRenderer.sprite = hitSprite;
                tween = DOVirtual.DelayedCall(0.1f, () => spriteRenderer.sprite = baseSprite);
            }
            else if (hitType == HitType.Color)
            {
                // change color, reset color in few seconds
                tween = DOTween.Sequence();
                (tween as Sequence)
                    .Append(spriteRenderer.DOColor(hitColor, 0.1f))
                    .Append(spriteRenderer.DOColor(baseColor, 0.1f));
            }
            else if (hitType == HitType.Material)
            {
                spriteRenderer.material = hitMaterial;
                tween = DOVirtual.DelayedCall(0.1f, () => spriteRenderer.material = baseMaterial);
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
                CameraManager.Instance.Shake();
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

            bool needSpriteRenderer = hitType == HitType.Sprite || hitType == HitType.Color || hitType == HitType.Material;

            if (needSpriteRenderer && spriteRenderer == null)
            {
                Debug.LogError("Hit type declared as Sprite or Color or Material but no sprite renderer provided");
                return;
            }
        }

        private void InitializeAttributes()
        {
            if (spriteRenderer)
            {
                baseColor = spriteRenderer.color;
                baseMaterial = spriteRenderer.material;
                baseSprite = spriteRenderer.sprite;
            }
            else
            {
                baseColor = Color.white;
            }
            baseScale = transform.localScale;
        }
    }
}
