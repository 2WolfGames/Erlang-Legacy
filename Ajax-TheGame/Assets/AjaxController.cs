using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Enums;

public class AjaxController : MonoBehaviour
{
    [Header("Linked")]
    [SerializeField] DashAttack dashAttack;
    [SerializeField] VengefulRay vengefulRay;

    [Header("Configurations")]
    [SerializeField] float collideRecoverTime = 1.5f;

    Collider2D ajaxCollider;
    AjaxMovement ajaxMovement;
    AjaxFX ajaxFX;
    LifeController lifeController;
    Touchable ajaxTouchable;
    Orientation ajaxOrientation;
    AjaxAttack ajaxAttack;

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

    void Awake()
    {
        ajaxMovement = GetComponent<AjaxMovement>();
        ajaxFX = GetComponent<AjaxFX>();
        lifeController = GetComponent<LifeController>();
        ajaxTouchable = GetComponent<Touchable>();
        ajaxOrientation = GetComponent<Orientation>();
        ajaxAttack = GetComponent<AjaxAttack>();

        ajaxCollider = GetComponent<Collider2D>();
    }

    // pre: --
    // post: change Ajax freeze state
    // desc: while freeze is setted to true, some Ajax features should be detached
    //      Ajax should not be able to move, or trigger it's habilities
    //      this, method are called at start and end of hit animations
    public void SetFreeze(bool freeze)
    {
        this.freeze = freeze;

        if (this.freeze)
        {
            // detach scripts
            ajaxMovement.Freeze();
            ajaxMovement.enabled = false;
            ajaxOrientation.enabled = false;
            ajaxAttack.enabled = false;
        }
        else
        {
            // attach scripts
            ajaxMovement.enabled = true;
            ajaxOrientation.enabled = true;
            ajaxAttack.enabled = true;
        }
    }

    // pre: --
    // post: take damage from collision
    public void CollidingWith(float collisionDamage, CollisionSide collisionSide, Collider2D other = null)
    {
        if (!ajaxTouchable.CanBeTouch) return;

        StartCoroutine(ajaxTouchable.UntouchableForSeconds(collideRecoverTime));

        ajaxFX.TriggerCollidingFX(collideRecoverTime, collisionSide);
        lifeController.TakeLife(Mathf.Abs(collisionDamage));

        Debug.Log($"Current ajax life ${lifeController.Life}");
    }

    public void Dash(float dashTime)
    {
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(false));
        StartCoroutine(ajaxTouchable.CanBeTouchCoroutine(true, dashTime));

        StartCoroutine(ajaxFX.CanFlipCoroutine(false));
        StartCoroutine(ajaxFX.CanFlipCoroutine(true, dashTime));
        StartCoroutine(ajaxFX.DashCoroutine(dashTime));

        StartCoroutine(ajaxMovement.DashCoroutine(FacingTo(), dashTime));

        StartCoroutine(dashAttack.AttackCoroutine(dashTime));
    }

    public void Ray(Vector3 origin)
    {
        bool left = FacingTo() == Facing.LEFT;
        Vector2 orientation = new Vector2(left ? -1f : 1f, 0f);
        VengefulRay instance = Instantiate(vengefulRay, origin, left ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
        instance.orientation = orientation;
    }

    // pre: --
    // post: remove other animations & goes to idle animation
    public void Idle()
    {
        // may to implement later?
    }

    public void Run(bool run)
    {
        ajaxFX.SetRunFX(run);
    }

    public void Land()
    {
        ajaxFX.TriggerLandFX();
    }

    public void Jump()
    {
        ajaxFX.TriggerJumpFX();
    }

    // pre: --
    // returns: Ajax's collider
    public Collider2D GetCollider()
    {
        return ajaxCollider;
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

    public Facing FacingTo()
    {
        return ajaxOrientation.LatestFacing;
    }

}
