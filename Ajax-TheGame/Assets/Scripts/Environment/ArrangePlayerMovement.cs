using System.Collections;
using System.Collections.Generic;
using Core.Player.Controller;
using UnityEngine;


public class ArrangePlayerMovement : MonoBehaviour
{

    [SerializeField] float velocityAlterator = 0.1f;
    GameObject ajax;
    float ajaxLastXPos;
    float platformLastXPos;

    public void Start()
    {
        platformLastXPos = transform.position.x;
    }

    //pre: --
    //post: every frame evaluates CalculatePlayerVelocity() and updates ajax and platform positions
    public void FixedUpdate()
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
    //if do so, corrects the alterations of player movement using ModifyVelocity() from AjaxMovemnt. // todo: mirar velocity modifier
    private void CalculatePlayerVelocity()
    {
        float ajaxDir = ajax.transform.localPosition.x - ajaxLastXPos;
        float platformDir = transform.position.x - platformLastXPos;

        //if player and platform are moving 
        if (ajaxDir != 0 && platformDir != 0)
        {
            if (ajaxDir < 0 && platformDir < 0 || ajaxDir > 0 && platformDir > 0)
            { //same dir
                // ajax.GetComponent<PlayerMovementManager>().ModifyVelocity(new Vector2(1 - velocityAlterator, 1));
                ajax.GetComponent<MovementController>().Acceleration = 1 - velocityAlterator;
            }
            else
            { // diferent dir
                // ajax.GetComponent<PlayerMovementManager>().ModifyVelocity(new Vector2(1 + velocityAlterator, 1));
                  ajax.GetComponent<MovementController>().Acceleration = 1 + velocityAlterator;
            }
        } else {
            ajax.GetComponent<MovementController>().Acceleration = 1;
        }
    }

    //pre: --
    //post: if collider is player then its parented with platform and we Activaate parent Mechanism
    public void OnTriggerEnter2D(Collider2D other)
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
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ajax = null;
            other.transform.parent = null; //Ajax no more Child of the platform
        }
    }
}
