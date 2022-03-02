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
    [Range(0.5f, 3.0f)][SerializeField] float timeBtwAttack;

    [Range(0.1f, 0.5f)][SerializeField] float dashAttackTime;

    [SerializeField] DashAttack dashAttack;

    float _timeBtwAttack = -1;

    AjaxMovement ajaxMovement;

    AjaxFX ajaxFX;

    [Header("Self")]
    [SerializeField] TangibleController selfTangibleController;


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
                this.ajaxFX.BlockOrientationChanges = true;
                ajaxMovement.Dash(Mathf.RoundToInt(transform.localScale.x), dashAttackTime, () =>
                {
                    this.ajaxFX.BlockOrientationChanges = false;
                });
                selfTangibleController.MakeNonTangible(dashAttackTime);
                dashAttack.ApplyDamage(dashAttackTime);
            }
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

}
