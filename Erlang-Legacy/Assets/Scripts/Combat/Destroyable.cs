﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat
{
    [RequireComponent(typeof(Health))]
    public class Destroyable : Hittable
    {
        private Health health => GetComponent<Health>();
        private int CurrentHealth => health.HP;
        public bool Invincible { get; set; }
        public bool IsDestroyed => health.HP <= 0;
        public bool IsAlive => !IsDestroyed;
        public UnityEvent OnDestroyed, OnRevived;

        public override void Awake()
        {
            base.Awake();
            Invincible = false;
        }

        public void OnAttackHit(Vector2 hitDirection, int damage)
        {
            if (CurrentHealth <= 0 || Invincible)
                return;

            DealDamage(damage);

            base.OnAttackHit(hitDirection); // Order of this call is important
        }

        public void DealDamage(int damage)
        {
            var destroyed = health.TakeHP(damage);
            if (destroyed)
                OnDestroyed?.Invoke();
        }

        public void Revive()
        {
            health.Revive();
            OnRevived?.Invoke();
        }
    }
}