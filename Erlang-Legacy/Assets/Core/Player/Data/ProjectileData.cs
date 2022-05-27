using System;
using Core.Combat.Projectile;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class ProjectileData
    {
        [SerializeField] RayProjectile projectile;
        [SerializeField] Transform origin; // from which point rays are triggered
        [SerializeField] float speed; // default ray ability speed
        [SerializeField] float lifetime; // default ray abilty duration before it get's destroyed 

        public Transform Origin { get => origin; set => origin = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Lifetime { get => lifetime; set => lifetime = value; }
        public RayProjectile Projectile { get => projectile; set => projectile = value; }
    }
}

