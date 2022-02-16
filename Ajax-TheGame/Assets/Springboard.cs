using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    Animator animator;

    [Range(5f, 100f)] [SerializeField] float force = 10f;

    [Range(-45, 45)] [SerializeField] float phi = 0;

    [Tooltip("Angle respect world to apply force")] HashSet<GameObject> memory = new HashSet<GameObject>();

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (memory.Contains(other.gameObject)) return;

        AjaxMovement ajaxMovement = other.GetComponent<AjaxMovement>();

        // var angle = phi * -1 + 90;
        // var xForce = Mathf.Cos((angle * 180) / Mathf.PI);
        // var yForce = Mathf.Sin((angle * 180) / Mathf.PI);


        ajaxMovement.ImpulseUp(force);

        memory.Add(other.gameObject);
        animator.SetBool("EXPAND", true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!memory.Contains(other.gameObject)) return;
        memory.Remove(other.gameObject);
        animator.SetBool("EXPAND", false);
    }
}