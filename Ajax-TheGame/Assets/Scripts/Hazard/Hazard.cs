using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Effect;
using Core.Character.Player;

// desc:
//  this scripts is usufull to hurt player when a collision is made
namespace Core.Hazard
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] int damage;

        void Update()
        {
            CheckCollision();
        }

        // pre: --
        // post: returns true if current colliders is touching collider's player
        bool IsTouchingPlayer()
        {
            var myCollider = GetComponent<Collider2D>();
            var playerCollider = BasePlayer.Instance?.GetComponent<Collider2D>();
            return playerCollider ? myCollider.IsTouching(playerCollider) : false;
        }

        // pre: --
        // post: if current object is colliding with enemy applies damamge
        void CheckCollision()
        {
            if (!IsTouchingPlayer()) return;
            var player = BasePlayer.Instance;
            if (!player.CanBeHit) return;
            player.Hurt(damage, gameObject);
        }
    }
}
