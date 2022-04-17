using System.Collections;
using UnityEngine;

using Core.Shared.Enum;

namespace Core.Character.Player
{
    public class FXController : MonoBehaviour
    {
        [Header("Linked")]
        [SerializeField] ParticleSystem jumpParticles;
        BasePlayer basePlayer;
        Animator animator;

        public bool CanFlip
        {
            get
            {
                var controllable = BasePlayer.Instance.Controllable;
                var isDashing = BasePlayer.Instance.IsDashing;
                return controllable && !isDashing;
            }
        }

        void Awake()
        {
            basePlayer = GetComponent<BasePlayer>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }

        void FixedUpdate()
        {
            if (CanFlip)
                Flip(BasePlayer.Instance.Facing);
        }

        // -1 left, 1 right 
        public void Flip(PlayerFacing facing)
        {
            Vector3 characterScale = transform.localScale;
            characterScale.x = facing == PlayerFacing.Left ? -1 : 1;
            transform.localScale = characterScale;
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
            // animator.SetBool("jumping", true);
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
        //          blink animation is in 2nd layer basePlayer
        private IEnumerator BlinkCoroutine(float seconds)
        {
            animator.SetBool("blink", true);
            yield return new WaitForSeconds(seconds);
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

        // public IEnumerator DashCoroutine(float dashDuration)
        // {
        //     animator.SetTrigger("dash");
        //     dashTrailRenderer.widthMultiplier = 3;
        //     yield return new WaitForSeconds(dashDuration);
        //     dashTrailRenderer.widthMultiplier = 0;
        // }

        public void SetRunFX(bool isRunning)
        {
            animator.SetBool("running", isRunning);
        }
    }
}
