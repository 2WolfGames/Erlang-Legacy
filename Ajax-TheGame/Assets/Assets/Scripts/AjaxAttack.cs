using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxAttack : MonoBehaviour
{
    [SerializeField] float timeBtwAttack;

    [SerializeField] Transform attackRef;

    [SerializeField] Vector2 basicAttackDimentions;

    [SerializeField] float basicAttackComputeDelay = 0.05f;

    [SerializeField] LayerMask whatIsEnemy;

    float _timeBtwAttack;

    void Update()
    {
        if (_timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(
                    ComputeLayersOnRange((Collider2D[] colliders) =>
                    {
                        Vector3 forward = transform.forward;
                        foreach (Collider2D collider in colliders)
                        {
                            Enemy enemy = collider.GetComponent<Enemy>();
                            if (enemy)
                            {
                                enemy.Harakiri(new Vector2(forward.z, 0));
                            }
                        }
                    })
                );
            }
            _timeBtwAttack -= Time.deltaTime;
        }
        else
        {
            _timeBtwAttack = timeBtwAttack;
        }
    }

    IEnumerator ComputeLayersOnRange(System.Action<Collider2D[]> callback)
    {
        yield return new WaitForSeconds(basicAttackComputeDelay);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attackRef.position, basicAttackDimentions, 0, whatIsEnemy);
        callback(enemies);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackRef.position, basicAttackDimentions);
    }
}
