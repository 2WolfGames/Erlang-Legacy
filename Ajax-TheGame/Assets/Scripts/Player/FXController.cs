using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

using Core.Shared.Enum;

namespace Core.Player
{
    public class FXController : MonoBehaviour
    {
        [Header("Linked")]
        [SerializeField] ParticleSystem jumpParticles;
        [SerializeField] TrailRenderer dashTrailRenderer;

        Controller controller;
        Animator animator;

        bool canFlip = true;

        bool blinking = false;

        public bool CanFlip
        {
            get
            {
                return canFlip;
            }
        }

        void Awake()
        {
            controller = GetComponent<Controller>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }

        void Start()
        {
            dashTrailRenderer.widthMultiplier = 0;
        }

        void FixedUpdate()
        {
            FlipListener();
        }

        void FlipListener()
        {
            if (!canFlip) return;
            UpdateFlip(controller.FacingTo());
        }

        // -1 left, 1 right 
        void UpdateFlip(PlayerFacing facing)
        {
            Vector3 characterScale = transform.localScale;
            characterScale.x = facing == PlayerFacing.Left ? -1 : 1;
            transform.localScale = characterScale;
        }

        // pre: not other coroutine of this fn should be running
        // post: inhibit flip actions for a while
        public IEnumerator InhibitFlip(float seconds = 0)
        {
            canFlip = false;
            yield return new WaitForSeconds(seconds);
            canFlip = true;
        }

        /**
            This method should trigger
            jump view and sound effects
        */
        public void TriggerJumpFX()
        {
            if (this.jumpParticles)
            {
                jumpParticles.Play();
            }
            animator.SetTrigger("jump");
            animator.SetBool("jumping", true);
        }

        // pre: --
        // post: executes `onComplete` func if ever is declared when animation is not playing
        private IEnumerator CheckAnimationCompleted(string animationName, System.Action onComplete)
        {
            while (true)
            {
                bool playing = animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
                if (!playing) break;
            }
            yield return null;
            if (onComplete != null) onComplete();
        }

        // pre: --
        // post: executes randomly hit animations & blink animations for a while
        public void TriggerCollidingFX(float blinkTime, Side side)
        {
            if (side == Side.Back)
            {
                animator.SetTrigger("twist");
            }
            else
            {
                animator.SetTrigger("hit");
            }
            StartCoroutine(BlinkCoroutine(blinkTime));
        }


        // pre: coroutine should not be called previously
        // post: trigger blink animations for x seconds
        //          blink animation is in 2nd layer controller
        private IEnumerator BlinkCoroutine(float seconds)
        {
            if (blinking) yield return null;
            blinking = true;
            animator.SetBool("blink", true);
            yield return new WaitForSeconds(seconds);
            blinking = false;
            animator.SetBool("blink", false);
        }

        /**
                This method should trigger
                land view and sound effects
            */
        public void TriggerLandFX()
        {
            //TODO: land particles
            animator.SetBool("jumping", false);
        }

        public IEnumerator DashCoroutine(float dashDuration)
        {
            animator.SetTrigger("dash");
            dashTrailRenderer.widthMultiplier = 3;
            yield return new WaitForSeconds(dashDuration);
            dashTrailRenderer.widthMultiplier = 0;
        }

        public void SetRunFX(bool isRunning)
        {
            animator.SetBool("running", isRunning);
        }
    }
}
