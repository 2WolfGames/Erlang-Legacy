using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Shared.Enum;
using Core.Shared;

using Core.Player;
namespace Enemy
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [Header("Configurations")]
        [Range(1, 1000)][SerializeField] int collisionDamage = 10;
        [Range(1, 1000)][SerializeField] int basicDamage = 10;
        [Range(0.0f, 0.5f)][SerializeField] float deadDelay = 0.1f;
        [SerializeField] LayerMask whatIsAjax;
        Controller ajaxController;
        Collider2D collider2d;
        LifeController lifeController;

        void Awake()
        {
            collider2d = GetComponent<Collider2D>();
            lifeController = GetComponent<LifeController>();
        }

        void Start()
        {
            ajaxController = FindObjectOfType<Controller>();
        }

        void FixedUpdate()
        {
            if (IsTouchingAjax(ajaxController.GetCollider()))
            {
                CollidingWithAjax(ajaxController);
            }
        }

        bool IsTouchingAjaxByLayer()
        {
            return collider2d.IsTouchingLayers(whatIsAjax);
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
        protected void CollidingWithAjax(Controller ajax)
        {
            Side side = Function.CollisionSide(ajax.transform, transform);
            if (ajax.CanBeTouch())
                ajax.CollidingWith(collisionDamage, side);
        }

        public void OnDie()
        {
            Debug.Log("OnHit@Enemy: die");
            Destroy(gameObject, deadDelay);
        }

        public bool OnHit(int damage)
        {
            if (lifeController == null)
            {
                Debug.Log("'LifeController' reference not attached");
                return true;
            }

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

}