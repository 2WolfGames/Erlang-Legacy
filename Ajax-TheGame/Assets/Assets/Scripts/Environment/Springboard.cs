using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    [Tooltip("Impulse force to add")]
    [Range(5f, 100f)][SerializeField] float force = 10f;

    [Tooltip("Util for diff between elements of same layer")]
    [SerializeField] string compareTag = "Player";

    [Tooltip("Deviation angle respect normal object")]
    [SerializeField] float desviation = 90;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /**
        Computes local rotation from
        it's nearest parent and adds
        deviations `phi` desviation, by default it's 90 degree
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (compareTag != null && !other.CompareTag(compareTag)) return;

        AjaxMovement ajaxMovement = other.GetComponent<AjaxMovement>();

        var rad = (transform.localEulerAngles.z + desviation) * Mathf.Deg2Rad;
        var xForce = force * Mathf.Cos(rad);
        var yForce = force * Mathf.Sin(rad);

        ajaxMovement.Impulse(new Vector2(xForce, yForce));

        animator.SetBool("EXPAND", true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (compareTag != null && !other.CompareTag(compareTag)) return;

        animator.SetBool("EXPAND", false);
    }
}
