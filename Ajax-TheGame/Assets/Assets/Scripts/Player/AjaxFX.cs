using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxFX : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] ParticleSystem vengefulLightParticles;
    [SerializeField] ParticleSystem electricity;


    [Header("Configurations")]
    [SerializeField] float electricityVFXDuration = 1f;


    [Header("Others")]
    [SerializeField] AjaxFacing ajaxFacing;
    [SerializeField] Animator ajaxAnimator;
    [SerializeField] TrailRenderer dashTrailRenderer;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    int demonElectrivity = 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public bool blockOrientationChanges
    {
        set; get;
    }

    void Start()
    {
        dashTrailRenderer.widthMultiplier = 0;
    }

    void FixedUpdate()
    {
        if (!blockOrientationChanges)
        {
            int orientation = ajaxFacing.LatestFacingToNumber();

            UpdateCharacterOrientation(orientation);
        }
    }

    // -1 left, 1 right 
    void UpdateCharacterOrientation(int orientation)
    {
        if (orientation != -1 && orientation != 1) return;

        Vector3 characterScale = transform.localScale;
        characterScale.x = orientation;
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
            this.jumpParticles.Play();
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

    public void TriggerVengefulFX()
    {
        StartCoroutine(ElectricityFX(this.electricityVFXDuration));
    }

    IEnumerator ElectricityFX(float duration)
    {
        demonElectrivity++;
        electricity.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        demonElectrivity--;

        if (demonElectrivity <= 0)
        {
            electricity.gameObject.SetActive(false);
        }
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
        StartCoroutine(ElectricityFX(dashDuration * 2));
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
