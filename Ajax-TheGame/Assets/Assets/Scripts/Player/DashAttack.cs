using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [Range(10, 1000)] [SerializeField] float damage = 100;

    BoxCollider2D boxCollider;

    HashSet<GameObject> colliders = new HashSet<GameObject>();

    // on aweke we disable dash triggers
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        boxCollider.isTrigger = true;
    }

    public void ApplyDamage(float time)
    {
        if (boxCollider.enabled) return;
        StartCoroutine(Await(time));
    }

    /**
        this functions make sure of
        handle the first and last state of
        - collider detector
        - collider set
    */
    IEnumerator Await(float time)
    {
        boxCollider.enabled = true;
        colliders.Clear();
        yield return new WaitForSeconds(time);
        boxCollider.enabled = false;
        colliders.Clear();
    }

    /**
        configure using game properties
        those layers that can interact with this
        `Dash` object
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent(typeof(IEnemy)))
        {
            if (!colliders.Contains(other.gameObject))
            {
                colliders.Add(other.gameObject);
                IEnemy enemy = other.GetComponent<IEnemy>();
                enemy.OnHit(damage);
            }
        }
    }

}
