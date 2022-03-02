using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    basic enemy
*/
public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Configurations")]
    [Tooltip("How many damage does to player")]
    [Range(1f, 1000f)][SerializeField] float basicDamage = 10f;
    [Range(0.0f, 0.5f)][SerializeField] float deadDelay = 0.1f;

    [Header("Self")]
    [SerializeField] LifeController selfLifeController;

    public void OnDie()
    {
        Debug.Log("OnHit@Enemy: die");
        Destroy(gameObject, deadDelay);
    }

    public bool OnHit(float damage)
    {
        bool dead = selfLifeController.TakeLife(damage);
        if (dead)
        {
            OnDie();
        }
        return dead;
    }

    public void OnCollisionEnter2D(Collider2D other)
    {

    }

    public void OnAttack(Collider2D other)
    {
        throw new System.NotImplementedException();
    }
}
