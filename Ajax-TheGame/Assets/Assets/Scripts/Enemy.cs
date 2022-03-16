using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Enums = Utils.Enums;

public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Configurations")]
    [Range(1f, 1000f)][SerializeField] float collisionDamage = 10f;
    [Range(1f, 1000f)][SerializeField] float basicDamage = 10f;
    [Range(0.0f, 0.5f)][SerializeField] float deadDelay = 0.1f;

    AjaxController ajaxController;
    Collider2D collider2d;
    LifeController lifeController;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        lifeController = GetComponent<LifeController>();
    }

    void Start()
    {
        ajaxController = FindObjectOfType<AjaxController>();
    }

    void FixedUpdate()
    {
        if (IsTouchingAjax(ajaxController.GetCollider()))
        {
            CollidingWithAjax(ajaxController);
        }
    }

    protected bool IsTouchingAjax(Collider2D ajaxCollider)
    {
        return collider2d.IsTouching(ajaxCollider);
    }

    // pre: --
    // post: trigger Ajax function event
    //      to report a collision with `this`
    //      computes which side of Ajax `LEFT`
    //      or `RIGHT` is colliding with `this`
    protected void CollidingWithAjax(AjaxController ajax)
    {
        Enums.CollisionSide side = Utils.Functions.ComputeCollisionSide(ajax.transform, transform);
        if (ajax.CanBeTouch())
            ajax.CollidingWith(collisionDamage, side);
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
