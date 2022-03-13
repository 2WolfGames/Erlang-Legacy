﻿using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AjaxFX : MonoBehaviour
{
    [Header("Jump particles")]
    [SerializeField] ParticleSystem jumpParticles;

    [Header("Self")]
    [SerializeField] AjaxController ajaxController;
    [SerializeField] Animator ajaxAnimator;
    [SerializeField] TrailRenderer dashTrailRenderer;

    bool canFlip = true;

    bool blinking = false;

    public bool CanFlip
    {
        get { return canFlip; }
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
        UpdateFlip(ajaxController.FacingTo());
    }

    // -1 left, 1 right 
    void UpdateFlip(Utils.Facing facing)
    {
        Vector3 characterScale = transform.localScale;
        characterScale.x = facing == Utils.Facing.LEFT ? -1 : 1;
        transform.localScale = characterScale;
    }

    public IEnumerator CanFlipCoroutine(bool flip, float time = 0)
    {
        yield return new WaitForSeconds(time);
        canFlip = flip;
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
        ajaxAnimator.SetTrigger("jump");
        ajaxAnimator.SetBool("jumping", true);
    }

    // pre: --
    // post: executes `onComplete` func if ever is declared when animation is not playing
    private IEnumerator CheckAnimationCompleted(string animationName, System.Action onComplete)
    {
        while (true)
        {
            bool playing = ajaxAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
            if (!playing) break;
        }
        yield return null;
        if (onComplete != null) onComplete();
    }

    // pre: --
    // post: executes randomly hit animations & blink animations for a while
    public void TriggerCollidingFX(float blinkSeconds = 5f)
    {
        int animation = Random.Range(1, 3);
        ajaxAnimator.SetTrigger("hit" + animation);
        StartCoroutine(BlinkFX(blinkSeconds));
    }


    // pre: coroutine should not be called previously
    // post: trigger blink animations for x seconds
    //          blink animation is in 2nd layer controller
    private IEnumerator BlinkFX(float seconds)
    {
        if (blinking) yield return null;
        blinking = true;
        ajaxAnimator.SetBool("blink", true);
        yield return new WaitForSeconds(seconds);
        Debug.Log("Blink awake");
        blinking = false;
        ajaxAnimator.SetBool("blink", false);
    }

    /**
            This method should trigger
            land view and sound effects
        */
    public void TriggerLandFX()
    {
        //TODO: land particles
        ajaxAnimator.SetBool("jumping", false);
    }

    /**
        This method should trigger
        land view and sound effects
    */
    public void TriggerDashFX(float dashDuration)
    {
        //TODO: land particles
        // TODO: (fix) dash animation can be longer than actual dash duration
        ajaxAnimator.SetTrigger("dash");
        StartCoroutine(IDashFX(dashDuration));
    }

    IEnumerator IDashFX(float dashDuration)
    {
        dashTrailRenderer.widthMultiplier = 3;
        yield return new WaitForSeconds(dashDuration);
        dashTrailRenderer.widthMultiplier = 0;
    }

    /**
        This method should trigger
        run/idle view and sound effects
    */
    public void SetRunFX(bool isRunning)
    {
        ajaxAnimator.SetBool("running", isRunning);
    }

}
