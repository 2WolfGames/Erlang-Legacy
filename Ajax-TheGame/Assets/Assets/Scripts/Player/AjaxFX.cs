using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxFX : MonoBehaviour
{
    [Header("Jump particles")]
    [SerializeField] ParticleSystem jumpParticles;

    // [SerializeField] Transform jumpParticlesTransform;

    [Header("Self")]
    [SerializeField] AjaxController ajaxController;
    [SerializeField] Animator ajaxAnimator;
    [SerializeField] TrailRenderer dashTrailRenderer;

    bool canFlip = true;

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

    public IEnumerator UpdateCanFlip(bool flip, float time = 0)
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
            // var particles = Instantiate(jumpParticles, jumpParticlesTransform.position, Quaternion.identity);
            jumpParticles.Play();
            // Destroy(particles.gameObject, 1f);
        }
        ajaxAnimator.SetTrigger("jump");
        ajaxAnimator.SetBool("jumping", true);
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
