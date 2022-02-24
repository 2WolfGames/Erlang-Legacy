using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlatform : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Rigidbody2D structure;
    [SerializeField] float speed;
    [SerializeField] float waitTime;
    [SerializeField] bool goBackwards;
    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        structure.transform.position = pointA.position;
        target = null;
    }

    void FixedUpdate()
    {
        if (target){
            var currentPos = structure.transform.position;
            structure.transform.position = Vector2.MoveTowards(currentPos, target.position, speed * Time.deltaTime);

            if (structure.transform.position == pointB.position && goBackwards)
            {
                StartCoroutine(GoBack(this.waitTime));
            }
        }

    }

    public IEnumerator GoBack(float time)
    {
        Debug.Log("About to go back");
        yield return new WaitForSeconds(time);
        Debug.Log("Going back!! ");
        if (structure.transform.position == pointB.position) {
            target = pointA;
        }
    }

    public void MoveStructure()
    {
        // estoy en el punto inicial?
        if (structure.transform.position == pointA.position)
        {
            target = pointB;
        }
        else if (structure.transform.position == pointB.position)
        {
            target = pointA;
        }

    }
}
