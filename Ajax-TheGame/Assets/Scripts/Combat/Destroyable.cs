﻿using System;
using UnityEngine;


// TODO: use health script component instead of local health state

namespace Core.Combat
{
    [RequireComponent(typeof(Health))]
    public class Destroyable : Hittable
    {
        [SerializeField] int health = 10;

        public int CurrentHealth { get => health; set => health = value; }
        public bool Invincible { get; set; }
        public bool IsDestroyed => health <= 0;
        public bool IsAlive => !IsDestroyed;
        public event Action OnDestroyed;

        public override void Awake()
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
