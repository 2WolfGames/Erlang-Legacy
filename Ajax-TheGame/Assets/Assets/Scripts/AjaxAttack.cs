using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxAttack : MonoBehaviour
{
    [Range(0.5f, 3.0f)] [SerializeField] float timeBtwAttack;

    [Range(0.0f, 1f)] [SerializeField] float dashAttackAwait = 0.01f;

    [SerializeField] DashAttack collidersDetector;

    [SerializeField] float dashDamage = 10f;

    [SerializeField] LayerMask whatIsEnemy;

    float _timeBtwAttack = -1;

    AjaxMovement ajaxMovement;

    AjaxFX ajaxFX;

    void Start()
    {
        ajaxMovement = GetComponent<AjaxMovement>();
        ajaxFX = GetComponent<AjaxFX>();
    }

    void Update()
    {
        if (_timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector2 direction = new Vector2(transform.localScale.x, 0);
                _timeBtwAttack = timeBtwAttack;
                this.ajaxFX.blockOrientationChanges = true;
                this.ajaxFX.spawnDashEcho = true;
                ajaxMovement.Dash(Mathf.RoundToInt(transform.localScale.x), () =>
                {
                    this.ajaxFX.spawnDashEcho = false;
                    this.ajaxFX.blockOrientationChanges = false;
                });
                ApplyDamageInDashRange(collidersDetector, dashAttackAwait);
            }
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

    void HitEnemies(List<Collider2D> enemiesColliders)
    {
        foreach (Collider2D collider in enemiesColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy)
            {
            }
        }
    }

    void ApplyDamageInDashRange(DashAttack detector, float await)
    {
        // compute colliders
        detector.ComputeCollidersDuring(await, (HashSet<IEnemy> entities) =>
        {
            foreach (IEnemy obj in entities)
            {
                obj.OnHit(100);
            }
        });
    }

}
