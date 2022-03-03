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
    [Range(0.5f, 3.0f)][SerializeField] float timeBtwAttack = 1.25f;

    [Range(0.1f, 0.5f)][SerializeField] float dashTime = 0.2f;

    float _timeBtwAttack = -1;

    [Header("Self")]
    [SerializeField] AjaxController ajaxController;

    void Update()
    {
        if (_timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ajaxController.OnDashAttack(dashTime);
            }
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

}
