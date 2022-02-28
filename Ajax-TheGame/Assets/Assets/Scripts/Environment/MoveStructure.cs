using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStructure : MonoBehaviour
{
    [SerializeField] Transform pointA; //initial point. 
    [SerializeField] Transform pointB;
    [SerializeField] Rigidbody2D structure;
    [SerializeField] float speed;
    [SerializeField] float waitTime;
    [SerializeField] bool goBackwards;
    Transform target;

    //post: structure is on initial position. 
    void Start()
    {
        structure.transform.position = pointA.position;
        target = null;
    }

    //pre: --
    //If there is a target (position to go), updates platform new position
    //if platform has to go back and has arribed, calls the corutine to go to initial point
    void FixedUpdate()
    {
        if (target){
            var currentPos = structure.transform.position;
            structure.transform.position = Vector2.MoveTowards(currentPos, target.position, speed * Time.deltaTime);

            if (structure.transform.position == pointB.position && goBackwards)
            {
                StartCoroutine(GoBack(this.waitTime));
                target = null; //seting target to null so this if it don't executes every frame
            }
        }

    }

    //pre: time > 0
    //post: it waits for time and it sets target to initial point
    public IEnumerator GoBack(float time)
    {
        yield return new WaitForSeconds(time);
        target = pointA;
    }

    //pre: --
    //post: sets the new targent position depending on where the structure is.
    //      if structure is in position A, target is position B
    //      else target is position A (initial pos)
    public void Move()
    {
        if (structure.transform.position == pointA.position)
        {
            target = pointB;
        }
        else if (structure.transform.position == pointB.position)
        {
            target = pointA;
        }

    }

    //pre: -
    //post: it enables or disables this script.
    public void EnableMove(bool enable){
        this.enabled = enable;
        if (!enable){
            target = null;
        }
    }
}
