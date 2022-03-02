using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    basic enemy
*/
public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Configurations")]
    [Tooltip("How many damage does to player")]
    [Range(1f, 1000f)][SerializeField] float basicDamage = 10f;
    [Range(0.0f, 0.5f)][SerializeField] float deadDelay = 0.1f;

    [Header("Player")]
    [SerializeField] LayerMask whatIsAjax;

    [Header("Self")]
    [SerializeField] LifeController selfLifeController;
    [SerializeField] Collider2D selfCollider;

    void FixedUpdate()
    {
        if (IsTouchingAjax())
        {
            GameObject ajax = GameObject.Find("Ajax");
            OnCollideWithAjax(ajax);
        }
    }

    protected bool IsTouchingAjax()
    {
        return selfCollider.IsTouchingLayers(whatIsAjax);
    }

    /*
        checks if is touching Ajax
        if it does, applies damange for collision
    **/
    protected void OnCollideWithAjax(GameObject ajax)
    {
        if (ajax.CompareTag("Player"))
        {
            var tangible = ajax.GetComponent<TangibleController>().Tangible;
            if (tangible == TangibleController.TangibleEnum.TANGIBLE)
            {
                var lifeController = ajax.GetComponent<LifeController>();
                var tangibleController = ajax.GetComponent<TangibleController>();
                lifeController.TakeLife(this.basicDamage);
                tangibleController.MakeNonTangible();
            }
        }
    }

    public void OnDie()
    {
        Debug.Log("OnHit@Enemy: die");
        Destroy(gameObject, deadDelay);
    }

    public bool OnHit(float damage)
    {
        bool dead = selfLifeController.TakeLife(damage);
        if (dead)
        {
            OnDie();
        }
        return dead;
    }

    public void OnAttack(Collider2D other)
    {
        Debug.Log("NOT IMPLEMENTED YET");
    }
}
