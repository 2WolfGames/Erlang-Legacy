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


    [Header("Linked")]
    [SerializeField] DashAttack dashAttack;

    [SerializeField] VengefulRay vengefulRay;


    [Header("Configurations")]
    [SerializeField] float touchableTime = 0.5f;


    /**
        collides with some element that stunes Ajax
    */
    public void CollidingWith(float collisionDamage, Collider2D other = null)
    {
        if (!ajaxTouchable.CanBeTouch) return;

        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(false, 0));
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(true, touchableTime));
        ajaxLife.TakeLife(Mathf.Abs(collisionDamage));

        Debug.Log($"Current ajax life ${ajaxLife.Life}");
    }


    public void DashTrigger(float dashTime)
    {
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(false, 0));
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(true, dashTime));

        StartCoroutine(ajaxFX.CanFlipCoroutine(false, 0));
        StartCoroutine(ajaxFX.CanFlipCoroutine(true, dashTime));

        StartCoroutine(ajaxMovement.DashCoroutine(FacingTo(), dashTime));

        StartCoroutine(dashAttack.AttackCoroutine(dashTime));
    }

    public void VengefulRayTrigger(Vector3 origin)
    {
        bool left = FacingTo() == Utils.Facing.LEFT;
        Vector2 orientation = new Vector2(left ? -1f : 1f, 0f);
        VengefulRay instance = Instantiate(vengefulRay, origin, left ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
        instance.orientation = orientation;
    }

    public bool CanBeTouch()
    {
        return ajaxTouchable.CanBeTouch;
    }

    // -1 | 0 | 1
    public int HorizontalInputNormalized()
    {
        return ajaxOrientation.InputToNumber();
    }

    public Utils.Facing FacingTo()
    {
        return ajaxOrientation.LatestFacing;
    }

}
