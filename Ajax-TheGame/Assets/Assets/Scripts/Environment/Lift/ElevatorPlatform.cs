using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    private Elevator parent;
    void Start()
    {
        parent = transform.parent.GetComponent<Elevator>();
    }
 
    private void OnTriggerEnter2D(Collider2D other) {
        parent.ActivateMechanism(other);
        other.transform.parent = this.transform;
    }

    private void OnTriggerExit2D(Collider2D other) {
        other.transform.parent = null;
    }
}
