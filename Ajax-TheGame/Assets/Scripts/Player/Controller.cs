﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Shared;
using Core.Shared.Enum;
using Core.Player.Ability;
using Core.Player.Util;

namespace Core.Player
{
    public class Controller : MonoBehaviour
    {
        [Header("Linked")]
        [SerializeField] Dash dashAttack;
        [SerializeField] VengefulRay vengefulRay;

        [Header("Configurations")]
        [SerializeField] float collideRecoverTime = 1.5f;

        Collider2D ajaxCollider;
        MovementController ajaxMovement;
        FXController ajaxFX;
        LifeController lifeController;
        Touchable ajaxTouchable;
        Orientation ajaxOrientation;
        AbilityController abilityController;
        static Controller instance;

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

        public static Controller Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Controller>();
                }
                return instance;
            }
        }

        void Awake()
        {
            ajaxMovement = GetComponent<MovementController>();
            ajaxFX = GetComponent<FXController>();
            lifeController = GetComponent<LifeController>();
            ajaxTouchable = GetComponent<Touchable>();
            ajaxOrientation = GetComponent<Orientation>();
            abilityController = GetComponent<AbilityController>();
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
                abilityController.enabled = false;
            }
            else
            {
                // attach scripts
                ajaxMovement.enabled = true;
                ajaxOrientation.enabled = true;
                abilityController.enabled = true;
            }
        }

        // pre: --
        // post: take damage from collision
        public void CollidingWith(float collisionDamage, Side collisionSide, Collider2D other = null)
        {
            if (!ajaxTouchable.CanBeTouch) return;
            StartCoroutine(ajaxTouchable.UntouchableForSeconds(collideRecoverTime));
            ajaxFX.TriggerCollidingFX(collideRecoverTime, collisionSide);
        }

        public void AddLife(int amount)
        {
            lifeController.AddLife(amount);
        }

        public void TakeLife(int amount)
        {
            Debug.Log($"Taking life... {amount}");
            lifeController.TakeLife(amount);
        }

        public void Dash(float dashTime)
        {
            StartCoroutine(ajaxTouchable.UntouchableForSeconds(dashTime));
            StartCoroutine(ajaxFX.InhibitFlip(dashTime));
            StartCoroutine(ajaxFX.DashCoroutine(dashTime));
            StartCoroutine(ajaxMovement.DashCoroutine(FacingTo(), dashTime));
            StartCoroutine(dashAttack.AttackCoroutine(dashTime));
        }

        public void Ray(Vector3 origin)
        {
            bool left = FacingTo() == PlayerFacing.Left;
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

        public PlayerFacing FacingTo()
        {
            return ajaxOrientation.LatestFacing;
        }

    }

}

