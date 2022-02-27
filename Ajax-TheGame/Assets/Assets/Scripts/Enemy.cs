using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    basic enemy
*/
public class Enemy : MonoBehaviour, IEnemy
{
    LifeController lifeController;

    [Range(0.0f, 0.5f)] [SerializeField] float deadDelay = 0.1f;

    void Awake()
    {
        lifeController = GetComponent<LifeController>();
    }

    public void OnDie()
    {
        Debug.Log("OnHit@Enemy: die");
        Destroy(gameObject, deadDelay);
    }

    public bool OnHit(float damage)
    {
        bool dead = lifeController.TakeLife(damage);
        if (dead)
        {
            OnDie();
        }
        return dead;
    }

    public void OnAttack()
    {
        throw new System.NotImplementedException();
    }
}
