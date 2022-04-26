﻿using System;
using UnityEngine;

namespace Core.Combat
{
    public class Destructable : Hittable
    {
        [SerializeField] int health = 10;

        public int CurrentHealth { get; set; }
        public bool Invincible { get; set; }

        public event Action OnDestroyed;

        protected override void Awake()
        {
            base.Awake();
            CurrentHealth = health;
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
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                OnDestroyed?.Invoke();
            }
        }

        public void Revive()
        {
            CurrentHealth = health;
        }
    }
}