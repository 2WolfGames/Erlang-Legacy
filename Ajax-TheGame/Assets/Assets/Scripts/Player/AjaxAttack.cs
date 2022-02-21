using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Script thought to controll
    Ajax's attacks.

    Make use of other scripts to 
    create awesome attacks and link
    to this one to manage them.
*/
public class AjaxAttack : MonoBehaviour
{
    [Range(0.5f, 3.0f)] [SerializeField] float tbwDashAttack;

    [Range(0.1f, 0.5f)] [SerializeField] float dashAttackTime;

    [Range(0.5f, 3.0f)] [SerializeField] float tbwRayAttack = 0.2f;

    [Range(1f, 3f)] [SerializeField] float rayAttackTime = 0.1f;

    [SerializeField] DashAttack dashAttack;

    [SerializeField] VengefulRay vengefulRay;

    float tbwDash = -1;

    AjaxMovement ajaxMovement;

    AjaxFX ajaxFX;

    bool inDashingAttack = false;

    void Start()
    {
        ajaxMovement = GetComponent<AjaxMovement>();
        ajaxFX = GetComponent<AjaxFX>();
        tbwDash = tbwDashAttack;
    }

    void Update()
    {
        if (tbwDash <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector2 direction = new Vector2(transform.localScale.x, 0);
                tbwDash = tbwDashAttack;
                this.inDashingAttack = true;
                this.ajaxFX.blockOrientationChanges = true;
                ajaxMovement.Dash(Mathf.RoundToInt(transform.localScale.x), dashAttackTime, () =>
                {
                    this.ajaxFX.blockOrientationChanges = false;
                    this.inDashingAttack = false;
                });
                dashAttack.ApplyDamage(dashAttackTime);
            }
        }
        else
        {
            tbwDash -= Time.deltaTime;
        }

        if (!inDashingAttack && Input.GetButtonDown("Fire2"))
        {
            VengefulRay ray = Instantiate(vengefulRay, transform.position, Quaternion.identity);
            float x = transform.localScale.x;
            Vector2 orientation = new Vector2(Mathf.Sign(x), 0);
            ray.orientation = orientation;
            Debug.Log(orientation);
            Destroy(ray.gameObject, rayAttackTime);
        }
    }

}
