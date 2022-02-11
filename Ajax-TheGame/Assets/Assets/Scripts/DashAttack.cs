using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    BoxCollider2D boxCollider;

    HashSet<IEnemy> colliders = new HashSet<IEnemy>();

    // on aweke we disable dash triggers
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        boxCollider.isTrigger = true;
    }

    public void ComputeCollidersDuring(float time, System.Action<HashSet<IEnemy>> onComplete)
    {
        if (boxCollider.enabled) return;
        StartCoroutine(Await(time, onComplete));
    }

    IEnumerator Await(float time, System.Action<HashSet<IEnemy>> onComplete)
    {
        colliders.Clear();
        boxCollider.enabled = true;
        yield return new WaitForSeconds(time);
        boxCollider.enabled = false;
        onComplete(colliders);
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
            colliders.Add(other.GetComponent<Enemy>());
        }
    }


}
