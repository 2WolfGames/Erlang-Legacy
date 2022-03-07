using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    basic enemy
*/
public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Self")]
    [SerializeField] LifeController lifeController;
    [SerializeField] Collider2D collider2d;

    [Header("Configurations")]

    [SerializeField] string playerObject = "Ajax";
    [Range(1f, 1000f)][SerializeField] float collisionDamage = 10f;
    [Range(1f, 1000f)][SerializeField] float basicDamage = 10f;
    [Range(0.0f, 0.5f)][SerializeField] float deadDelay = 0.1f;

    GameObject ajax;
    Collider2D ajaxCollider;

    void Start()
    {
        ajax = GameObject.Find(playerObject);
        ajaxCollider = ajax.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (IsTouchingAjax())
        {
            CollidingWithAjax(ajax);
        }
    }

    protected bool IsTouchingAjax()
    {
        return collider2d.IsTouching(ajaxCollider);
    }

    protected void CollidingWithAjax(GameObject ajax)
    {
        AjaxController ajaxController = ajax.GetComponent<AjaxController>();

        if (ajaxController.CanBeTouch())
        {
            ajaxController.CollidingWith(collisionDamage, collider2d);
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
