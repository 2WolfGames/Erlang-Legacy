﻿using Core.Player.Controller;
using UnityEngine;

// desc:
//  this scripts is usufull to hurt player when a collision is made
namespace Core.Hazard
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] int damage = 1;

        public int Damage { get => damage; set => damage = value; }

        public void Update()
        {
            CheckCollision();
        }

        // pre: --
        // post: returns true if current colliders is touching collider's player
        bool IsTouchingPlayer()
        {
            var myCollider = GetComponent<Collider2D>();
            var playerCollider = PlayerController.Instance.BodyCollider;
            return myCollider.IsTouching(playerCollider);
        }

        // pre: --
        // post: if current object is colliding with enemy applies damamge
        void CheckCollision()
        {
            var player = PlayerController.Instance;
            if (!player.CanBeHit)
                return;
            if (!IsTouchingPlayer())
                return;
            player.Hurt(damage, gameObject);
        }
    }
}
