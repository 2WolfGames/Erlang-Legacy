using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxFX : MonoBehaviour
{
    enum Face
    {
        LEFT,
        RIGHT
    }

    [SerializeField] Animator ajaxAnimator;

    [SerializeField] ParticleSystem jumpParticles;

     [SerializeField] TrailRenderer dashTrailRenderer;

    float orientation = 0f;

    Face facing = Face.RIGHT;

    public bool spawnDashEcho
    {
        set; get;
    }

    public bool blockOrientationChanges
    {
        set; get;
    }

    void Start()
    {
        dashTrailRenderer.widthMultiplier  = 0;
    }

    void Update()
    {
        orientation = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (!blockOrientationChanges)
        {
            HandleCharacterOrientation();
        }
    }

    void HandleCharacterOrientation()
    {
        int orientation = Mathf.RoundToInt(this.orientation);
        UpdateCharacterOrientation(orientation);
    }

    // -1 left, 1 right 
    void UpdateCharacterOrientation(int orientation)
    {
        if (orientation != -1 && orientation != 1) return;

        this.facing = orientation == 1 ? Face.RIGHT : Face.LEFT;

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
        ajaxAnimator.SetBool("jumping",true);
    }

    /**
        This method should trigger
        land view and sound effects
    */
    public void TriggerLandFX()
    {
        //TODO: land particles
        ajaxAnimator.SetBool("jumping",false);
    }

    /**
        This method should trigger
        land view and sound effects
    */
    public void TriggerDashFX(float dashDuration)
    {
        //TODO: land particles
        ajaxAnimator.SetTrigger("dash");
        StartCoroutine(IDashFX(dashDuration));
        
    }

    IEnumerator IDashFX(float dashDuration){
        dashTrailRenderer.widthMultiplier  = 3;
        yield return new WaitForSeconds(dashDuration);
        dashTrailRenderer.widthMultiplier  = 0;
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
