using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxController : MonoBehaviour
{
    [Header("Self")]
    [SerializeField] AjaxMovement ajaxMovement;
    [SerializeField] AjaxTangible ajaxTangible;
    [SerializeField] AjaxFX ajaxFX;
    [SerializeField] LifeController ajaxLife;

    [Header("Configurations")]
    [SerializeField] float collisionNonTangibleTime = 0.5f;


    /**
        collides with some element that stunes Ajax
    */
    public void OnCollisionWith(float collisionDamage, GameObject other = null)
    {
        if (!IsTangible()) return;

        ajaxTangible.OnTemporaryNonTangible(collisionNonTangibleTime);
        ajaxLife.TakeLife(Mathf.Abs(collisionDamage));
    }

    public bool IsTangible()
    {
        return ajaxTangible.Tangible == AjaxTangible.TangibleEnum.TANGIBLE;
    }

    public void OnDashAttack(float dashTime)
    {
        // block ajax orientation
        // make non tangible
        // execute dash instruction
        // unblock ajax orientation

        ajaxTangible.OnTemporaryNonTangible(dashTime);
        ajaxFX.BlockOrientationChanges = true;
        // TODO: pick from Ajax facing to know the correct orientation
        ajaxMovement.Dash(1, dashTime, () => { ajaxFX.BlockOrientationChanges = false; });
        // TODO: use ajax dash script to make damage in area
    }

}
