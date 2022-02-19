using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    private Elevator parent;
    private GameObject ajax;
    private bool ajaxParented = false;

    void Start()
    {
        parent = transform.parent.GetComponent<Elevator>();
    }

    private void FixedUpdate() {
        if (ajax){
           if (ajaxParented && Input.GetAxisRaw("Horizontal") != 0){
               Debug.Log("not parented");
               ajax.transform.parent = null;
               ajaxParented = false;
           } 
           
           if (!ajaxParented && Input.GetAxisRaw("Horizontal") == 0) {
               Debug.Log("parented");
               ajax.transform.parent = this.transform;
               ajaxParented = true;
           }
        }
    }
 
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ajax")){
            parent.ActivateMechanism(other);
            ajax= other.gameObject;
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ajax")){
            ajax = null;
            other.transform.parent = null;
        }
    }
}
