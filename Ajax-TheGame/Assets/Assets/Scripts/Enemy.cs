using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (IsTouchingAjax(ajaxController.AjaxCollider()))
        {
            CollidingWithAjax(ajaxController);
        }
    }

    protected bool IsTouchingAjax(Collider2D ajaxCollider)
    {
        return collider2d.IsTouching(ajaxCollider);
    }

    protected void CollidingWithAjax(AjaxController ajax)
    {
        if (ajax.CanBeTouch())
            ajax.CollidingWith(collisionDamage, collider2d);
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
