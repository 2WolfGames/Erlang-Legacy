using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxAttack : MonoBehaviour
{
    [SerializeField] float timeBtwAttack;

    [SerializeField] Transform attackRef;

    [SerializeField] float attackDistance;

    [SerializeField] float attackComputeDelay = 0.05f;

    [SerializeField] LayerMask whatIsEnemy;

    float _timeBtwAttack = 0;

    void Update()
    {
        if (_timeBtwAttack <= 0)
        {
            if (Input.GetButton("Fire1"))
            {
                Vector2 origin = transform.position;
                Vector2 direction = new Vector2(transform.forward.z, 0);
                StartCoroutine(ComputeEnemiesInRange(origin, direction, attackDistance, (enemies) => HitEnemies(enemies)));
            }
            _timeBtwAttack = timeBtwAttack;
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

    void HitEnemies(List<Collider2D> enemiesColliders)
    {
        Debug.Log(enemiesColliders.Count);
        foreach (Collider2D collider in enemiesColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Harakiri(new Vector2(transform.forward.z, 0));
            }
        }
    }

    IEnumerator ComputeEnemiesInRange(Vector2 origin, Vector2 direction, float distance, System.Action<List<Collider2D>> callback)
    {

        yield return new WaitForSeconds(attackComputeDelay);
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, whatIsEnemy);

        List<Collider2D> enemies = new List<Collider2D>();

        foreach (RaycastHit2D hit in hits)
        {
            enemies.Add(hit.collider);
        }
        // Collider2D[] enemies = Physics2D.OverlapBoxAll(attackRef.position, basicAttackDimentions, 0, whatIsEnemy);
        callback(enemies);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, new Vector2(transform.forward.z, 0) * attackDistance);
    }
}
