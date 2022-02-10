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

    [SerializeField] GameObject dashEcho;

    [SerializeField] float echoDashTimeBtwSpawn;

    [SerializeField] ParticleSystem jumpParticles;

    float _echoDashTimeBtwSpawn = 0.2f;

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
        this._echoDashTimeBtwSpawn = echoDashTimeBtwSpawn;
    }

    void Update()
    {
        orientation = Input.GetAxisRaw("Horizontal");

        if (spawnDashEcho)
        {
            if (_echoDashTimeBtwSpawn < Mathf.Epsilon)
            {
                Vector3 rot = transform.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y + 180, rot.z);
                var instance = Instantiate(this.dashEcho, transform.position, facing == Face.RIGHT ? Quaternion.identity : Quaternion.Euler(rot));
                Destroy(instance, echoDashTimeBtwSpawn + 0.1f);
                this._echoDashTimeBtwSpawn = echoDashTimeBtwSpawn;
            }
            else
            {
                this._echoDashTimeBtwSpawn -= Time.deltaTime;
            }
        }
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
    }

    /**
        This method should trigger
        land view and sound effects
    */
    public void TriggerLandFX()
    {
        //TODO: land particles
        ajaxAnimator.SetTrigger("land");
    }

        /**
        This method should trigger
        land view and sound effects
    */
    public void TriggerDashFX()
    {
        //TODO: land particles
        ajaxAnimator.SetTrigger("dash");
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
