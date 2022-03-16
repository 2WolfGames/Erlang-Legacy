using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemy
{
    /**
        thought to return true when ever the
        character was hit and die because of hit
    */
    bool OnHit(float damage);

    void OnDie();

    void OnAttack(Collider2D other);
}
