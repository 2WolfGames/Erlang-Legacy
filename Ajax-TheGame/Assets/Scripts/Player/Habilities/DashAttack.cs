using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [Header("Self")]
    [SerializeField] BoxCollider2D boxCollider;


    [Header("Configurations")]

    [Tooltip("Amount of life to take to enemies")]
    [Range(10, 1000)][SerializeField] int damage = 100;

    HashSet<GameObject> distinct = new HashSet<GameObject>();

    public IEnumerator AttackCoroutine(float time)
    {
        boxCollider.enabled = true;
        distinct.Clear();
        yield return new WaitForSeconds(time);
        boxCollider.enabled = false;
        distinct.Clear();
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
            if (!distinct.Contains(other.gameObject))
            {
                distinct.Add(other.gameObject);
                IEnemy enemy = other.GetComponent<IEnemy>();
                enemy.OnHit(damage);
            }
        }
    }

}
