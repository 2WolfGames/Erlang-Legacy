using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxController : MonoBehaviour
{
    [Header("Self")]
    [SerializeField] AjaxMovement ajaxMovement;
    [SerializeField] AjaxFX ajaxFX;
    [SerializeField] LifeController ajaxLife;
    [SerializeField] Touchable ajaxTouchable;
    [SerializeField] Orientation ajaxOrientation;


    [Header("Linked objects")]
    [SerializeField] DashAttack dashAttack;


    [Header("Configurations")]
    [SerializeField] float touchableTime = 0.5f;


    /**
        collides with some element that stunes Ajax
    */
    public void OnCollisionWith(float collisionDamage, GameObject other = null)
    {
        if (!ajaxTouchable.CanBeTouch) return;

        StartCoroutine(ajaxTouchable.UpdateCanBeTouch(false, 0));
        StartCoroutine(ajaxTouchable.UpdateCanBeTouch(true, touchableTime));
        ajaxLife.TakeLife(Mathf.Abs(collisionDamage));
    }


    public void TriggerDash(float dashTime)
    {
        StartCoroutine(ajaxTouchable.UpdateCanBeTouch(false, 0));
        StartCoroutine(ajaxTouchable.UpdateCanBeTouch(true, dashTime));

        StartCoroutine(ajaxFX.UpdateCanFlip(false, 0));
        StartCoroutine(ajaxFX.UpdateCanFlip(true, dashTime));

        StartCoroutine(ajaxMovement.Dash(ajaxOrientation.LatestFacing, dashTime));

        StartCoroutine(dashAttack.Attack(dashTime));
    }

    public void TriggerVengefulRay(Vector3 origin)
    {

    }

    public bool CanBeTouch()
    {
        return ajaxTouchable.CanBeTouch;
    }

    /**
    retruns a number { -1, 0, 1}
    **/
    public int XVelocityNormalized()
    {
        return ajaxOrientation.InputToNumber();
    }

    public Utils.Facing FacingTo()
    {
        return ajaxOrientation.LatestFacing;
    }

}
