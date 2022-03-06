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
    [Header("Self")]
    [SerializeField] AjaxController ajaxController;


    [Header("Dash configurations")]
    [Range(0.25f, 3.0f)][SerializeField] float timeBtwDash;
    [Range(0.1f, 0.5f)][SerializeField] float dashTime;


    [Header("Vengeful ray configuration")]
    [SerializeField] Transform vfRayTransform;
    [Range(0.25f, 3.0f)][SerializeField] float timeBtwVfRay = 0.5f;


    float memoTimeBtwDash;
    float memoTimeBtwVfRay;
    bool isDashing;


    void Awake()
    {
        memoTimeBtwDash = timeBtwDash;
        memoTimeBtwVfRay = timeBtwVfRay;
        isDashing = false;
    }

    void Update()
    {
        DashListener();
        VengefullRayListener();
    }

    void DashListener()
    {
        if (memoTimeBtwDash >= 0)
        {
            memoTimeBtwDash -= Time.deltaTime;
        }

        if (memoTimeBtwDash <= 0 && !isDashing && Input.GetButtonDown("Fire1"))
        {
            ResetMemoTimeBtwDash();
            isDashing = true;
            ajaxController.TriggerDash(dashTime);
            StartCoroutine(UpdateIsDashing(dashTime, false));
        }
    }

    void VengefullRayListener()
    {
        if (memoTimeBtwVfRay >= 0)
        {
            memoTimeBtwVfRay -= Time.deltaTime;
        }

        if (memoTimeBtwVfRay <= 0 && !isDashing && Input.GetButtonDown("Fire2"))
        {
            ResetMemoTimeBtwVfRay();
            ajaxController.TriggerVengefulRay(vfRayTransform.position);
        }
    }

    void ResetMemoTimeBtwDash()
    {
        memoTimeBtwDash = timeBtwDash;
    }

    void ResetMemoTimeBtwVfRay()
    {
        memoTimeBtwVfRay = timeBtwVfRay;
    }

    IEnumerator UpdateIsDashing(float time, bool state)
    {
        yield return new WaitForSeconds(time);
        isDashing = false;
    }


    // /**
    //     Handles dash frequency
    //     and comunicate with dash attack script
    //     to make demage in area
    // */
    // void DashDemon()
    // {
    //     if (tbwDash <= 0 && !this.inDashingAttack && Input.GetButtonDown("Fire1"))
    //     {
    //         tbwDash = tbwDashAttack;
    //         Vector2 direction = new Vector2(transform.localScale.x, 0);
    //         this.inDashingAttack = true;
    //         this.ajaxFX.BlockOrientationChanges = true;
    //         ajaxMovement.Dash(Mathf.RoundToInt(transform.localScale.x), dashAttackTime, () =>
    //         {
    //             this.ajaxFX.BlockOrientationChanges = false;
    //             this.inDashingAttack = false;
    //         });
    //         dashAttack.Attack(dashAttackTime);
    //     }
    // }


    // /**
    //     Handles vengeful ray frequency
    //     and instanciate one venful ray
    //     to make demage in area
    // */
    // void VengefulRayDemon()
    // {
    //     if (tbwRay <= 0 && !inDashingAttack && Input.GetButtonDown("Fire2"))
    //     {
    //         tbwRay = tbwVengefulRay;
    //         float x = transform.localScale.x;
    //         float facing = Mathf.Sign(x);
    //         Vector2 orientation = new Vector2(facing, 0);
    //         var ray = Instantiate(vengefulRay, vengefullRayStartPosition.position, facing == -1f ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
    //         ray.orientation = orientation;
    //     }
    // }


}
