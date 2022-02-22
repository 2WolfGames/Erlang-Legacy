using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IMechanism
{
    const float CONST_SPEED = 3f;                        

    [SerializeField] Transform endPoint;

    [SerializeField] GameObject doorStructure;

    bool moving = false; 

    Vector2 endPos;
    
    // Start is called before the first frame update
    void Start()
    {
        doorStructure.transform.position = new Vector3(doorStructure.transform.position.x, doorStructure.transform.position.y,0);
        endPos = new Vector2(endPoint.position.x, endPoint.position.y);
    }

    void FixedUpdate(){
        if (moving) {
            //moving structure to end pos
            Vector2 structurePos = new Vector2(doorStructure.transform.position.x,doorStructure.transform.position.y);
            doorStructure.transform.position = Vector2.MoveTowards(structurePos, endPos, CONST_SPEED * Time.deltaTime);

            if (structurePos == endPos) { 
                moving = false;
            }
        }
    }

    //pre: CanActivate()
    //post: prepares the variables to move the strucuture 
    public void Activate(Lever l){
        moving = true;
    }

    //pre: --
    //post: returs true is structure is not in end point
    // not moving  OR  structure is moving but hasToArribe is not activated and togglePoint is not the end
    public bool CanActivate(Lever l){
        return 
        doorStructure.transform.position != endPoint.position
        && !moving;
    }


}
