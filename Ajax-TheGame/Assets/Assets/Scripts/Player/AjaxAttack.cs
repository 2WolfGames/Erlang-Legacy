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
    [Header("Dash")]
    [Range(0.25f, 3.0f)][SerializeField] float timeBtwDash;
    [Range(0.1f, 0.5f)][SerializeField] float dashTime;


    [Header("Vengeful ray")]
    [Range(0.25f, 3.0f)][SerializeField] float timeBtwVfRay = 0.5f;

    [Header("Linked")]

    AjaxController ajaxController;
    [SerializeField] Transform vengefulRayTransform;

    float memoTimeBtwDash;
    float memoTimeBtwVfRay;
    bool isDashing;


    void Awake()
    {
        memoTimeBtwDash = timeBtwDash;
        memoTimeBtwVfRay = timeBtwVfRay;
        isDashing = false;
        ajaxController = GetComponent<AjaxController>();
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
            ajaxController.Dash(dashTime);
            StartCoroutine(DashingCoroutine(dashTime, false));
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
            ajaxController.Ray(vengefulRayTransform.position);
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

    // pre: --
    // post: set dashing state to true waits for n seconds and then set dashing to false
    IEnumerator DashingCoroutine(float time, bool state)
    {
        isDashing = true;
        yield return new WaitForSeconds(time);
        isDashing = false;
    }

}
