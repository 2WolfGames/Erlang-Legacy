using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    [Tooltip("Impulse force to add")]
    [Range(5f, 100f)] [SerializeField] float force = 10f;

    [Tooltip("Deviation angle respect normal object")]
    [SerializeField] float desviation = 90;

    ////////////////////////////////////////////////////////////////////////////////////////////////

    Animator animator;
    HashSet<GameObject> memory = new HashSet<GameObject>();

    ////////////////////////////////////////////////////////////////////////////////////////////////

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
        Debug.Log(other.name);

        if (memory.Contains(other.gameObject)) return;

        if (!other.CompareTag("Player")) return;

        AjaxMovement ajaxMovement = other.GetComponent<AjaxMovement>();

        var rad = (transform.localEulerAngles.z + desviation) * Mathf.Deg2Rad;
        var xForce = force * Mathf.Cos(rad);
        var yForce = force * Mathf.Sin(rad);

        ajaxMovement.Impulse(new Vector2(xForce, yForce));

        memory.Add(other.gameObject);
        animator.SetBool("EXPAND", true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(other.name);

        if (!memory.Contains(other.gameObject)) return;

        if (!other.CompareTag("Player")) return;

        memory.Remove(other.gameObject);
        animator.SetBool("EXPAND", false);
    }
}