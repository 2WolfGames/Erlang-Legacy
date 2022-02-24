using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureHandlePlayer : MonoBehaviour
{

    float velocityAlterator = 0.1f;

    private GameObject ajax;

    private float ajaxLastXPos;

    private float platformLastXPos;

    void Start()
    {

        platformLastXPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        if (ajax)
        {
            CalculatePlayerVelocity();
            ajaxLastXPos = ajax.transform.localPosition.x;
        }
        platformLastXPos = transform.position.x;
    }

    //pre: ajax != null
    //post: looks if player and platform are moving at the same time 
    //if do so, corrects the alterations of player movement using ModifyVelocity() from AjaxMovemnt.
    private void CalculatePlayerVelocity()
    {
        float ajaxDir = ajax.transform.localPosition.x - ajaxLastXPos;
        float platformDir = transform.position.x - platformLastXPos;

        if (ajaxDir != 0 && platformDir != 0)
        {
            if (ajaxDir < 0 && platformDir < 0 || ajaxDir > 0 && platformDir > 0)
            { //same dir
                ajax.GetComponent<AjaxMovement>().ModifyVelocity(new Vector2(1 - velocityAlterator, 1));
            }
            else
            { // diferent dir
                ajax.GetComponent<AjaxMovement>().ModifyVelocity(new Vector2(1 + velocityAlterator, 1));
            }
        }
    }

    //pre: --
    //post: if collider is player then its parented with platform and we Activaate parent Mechanism
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ajax = other.gameObject;
            ajax.transform.parent = this.transform; //Ajax as a Child of the platform
            ajaxLastXPos = ajax.transform.localPosition.x;
        }
    }

    //pre: --
    //post: if collider is player we unparent them
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ajax = null;
            other.transform.parent = null; //Ajax no more Child of the platform
        }
    }
}
