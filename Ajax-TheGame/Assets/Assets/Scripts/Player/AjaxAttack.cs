using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Script thought to controll
    Ajax's attacks.

    Make use of other scripts to 
    create awesome attacks and link
    to this one to manage them.
*/
public class AjaxAttack : MonoBehaviour
{
    [Header("Dash configurations")]
    [SerializeField] DashAttack dashAttack;
    [Range(0.25f, 3.0f)] [SerializeField] float tbwDashAttack;
    [Range(0.1f, 0.5f)] [SerializeField] float dashAttackTime;


    [Header("Vengeful ray configuration")]
    [SerializeField] VengefulRay vengefulRay;
    [SerializeField] Transform vengefullRayStartPosition;
    [Range(0.25f, 3.0f)] [SerializeField] float tbwVengefulRay = 0.5f;

    [Header("Others")]
    AjaxFacing ajaxFacing;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    float tbwDash = -1;
    float tbwRay = -1;
    AjaxMovement ajaxMovement;
    AjaxFX ajaxFX;
    bool inDashingAttack = false;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        ajaxMovement = GetComponent<AjaxMovement>();
        ajaxFX = GetComponent<AjaxFX>();
        // tbwDash = tbwDashAttack;
        // tbwRay = tbwVengefulRay;
    }

    void Update()
    {
        tbwDash = Mathf.Clamp(tbwDash - Time.deltaTime, Mathf.NegativeInfinity, tbwDashAttack);
        tbwRay = Mathf.Clamp(tbwRay - Time.deltaTime, Mathf.NegativeInfinity, tbwVengefulRay);
        DashDemon();
        VengefulRayDemon();
    }

    /**
        Handles dash frequency
        and comunicate with dash attack script
        to make demage in area
    */
    void DashDemon()
    {
        if (tbwDash <= 0 && !this.inDashingAttack && Input.GetButtonDown("Fire1"))
        {
            tbwDash = tbwDashAttack;
            Vector2 direction = new Vector2(transform.localScale.x, 0);
            this.inDashingAttack = true;
            this.ajaxFX.blockOrientationChanges = true;
            ajaxMovement.Dash(Mathf.RoundToInt(transform.localScale.x), dashAttackTime, () =>
            {
                this.ajaxFX.blockOrientationChanges = false;
                this.inDashingAttack = false;
            });
            dashAttack.Attack(dashAttackTime);
        }
    }


    /**
        Handles vengeful ray frequency
        and instanciate one venful ray
        to make demage in area
    */
    void VengefulRayDemon()
    {
        if (tbwRay <= 0 && !inDashingAttack && Input.GetButtonDown("Fire2"))
        {
            tbwRay = tbwVengefulRay;
            float x = transform.localScale.x;
            float facing = Mathf.Sign(x);
            Vector2 orientation = new Vector2(facing, 0);
            var ray = Instantiate(vengefulRay, vengefullRayStartPosition.position, facing == -1f ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
            ray.orientation = orientation;
        }
    }

}
