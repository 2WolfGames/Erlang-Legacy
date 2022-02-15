using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    Animator animator;

    HashSet<GameObject> memory = new HashSet<GameObject>();

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (memory.Contains(other.gameObject)) return;
        Debug.Log("*");
        memory.Add(other.gameObject);
        animator.SetBool("EXPAND", true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!memory.Contains(other.gameObject)) return;
        Debug.Log("?");
        memory.Remove(other.gameObject);
        animator.SetBool("EXPAND", false);
    }
}