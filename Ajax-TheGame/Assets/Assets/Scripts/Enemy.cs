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

    [Range(1f, 1000f)][SerializeField] float collisionDamage = 10f;
    [Range(1f, 1000f)][SerializeField] float basicDamage = 10f;
    [Range(0.0f, 0.5f)][SerializeField] float deadDelay = 0.1f;

    [Header("Player")]
    [SerializeField] LayerMask whatIsAjax;

    [Header("Self")]
    [SerializeField] LifeController lifeController;
    [SerializeField] Collider2D selfCollider;

    GameObject ajax;

    void Start()
    {
        ajax = GameObject.Find("Ajax");
    }

    void FixedUpdate()
    {
        if (IsTouchingAjax())
        {
            OnCollisionWithAjax(ajax);
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
    protected void OnCollisionWithAjax(GameObject ajax)
    {
        if (ajax.CompareTag("Player"))
        {
            AjaxController ajaxController = ajax.GetComponent<AjaxController>();

            if (ajaxController.CanBeTouch())
            {
                ajaxController.OnCollisionWith(collisionDamage);
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
        bool dead = lifeController.TakeLife(damage);
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
