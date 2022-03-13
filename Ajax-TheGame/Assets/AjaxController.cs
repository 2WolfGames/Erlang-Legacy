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

    // Ajax can not move & can not fire any hability
    // freeze state change when Ajax was hit by some enemy
    // trigger by hit1 & hit2 animation by now
    bool freeze = false;

    public bool Freeze
    {
        get
        {
            return freeze;
        }
    }

    // pre: --
    // post: change Ajax freeze state
    // desc: while freeze is setted to true, some Ajax features should be freezed
    //      Ajax should not be able to move, or trigger it's habilities
    //      this, method are called at start and end of hit animations
    public void UpdateFreeze(bool freeze)
    {
        this.freeze = freeze;
    }

    // pre: --
    // post: take damage because of collision with enemy
    //       make Ajax untouchable for `touchableTime` seconds
    // desc: 
    public void CollidingWith(float collisionDamage, Collider2D other = null)
    {
        if (!ajaxTouchable.CanBeTouch) return;

        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(false, 0));
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(true, touchableTime));

        Debug.Log(touchableTime);

        ajaxFX.TriggerCollidingFX(touchableTime);
        ajaxLife.TakeLife(Mathf.Abs(collisionDamage));

        Debug.Log($"Current ajax life ${ajaxLife.Life}");
    }


    public void DashTrigger(float dashTime)
    {
        if (freeze) return;

        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(false, 0));
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(true, dashTime));

        StartCoroutine(ajaxFX.CanFlipCoroutine(false, 0));
        StartCoroutine(ajaxFX.CanFlipCoroutine(true, dashTime));

        StartCoroutine(ajaxMovement.DashCoroutine(FacingTo(), dashTime));

        StartCoroutine(dashAttack.AttackCoroutine(dashTime));
    }

    public void VengefulRayTrigger(Vector3 origin)
    {
        if (freeze) return;

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
