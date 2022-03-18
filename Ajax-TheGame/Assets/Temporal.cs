using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporal : MonoBehaviour
{
    public void Hola()
    {
        Debug.Log("Hola from temporal");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
    }
}
