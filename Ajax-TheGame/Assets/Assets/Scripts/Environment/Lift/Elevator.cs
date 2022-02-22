using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour, IMechanism
{
    const float CONST_SPEED = 3f;

    const float CONST_WAIT_TIME = 3f;

    //The structure return to initial position/state
    [SerializeField] bool goToInitialState;

    //Structure Journey can be interrupted
    [SerializeField] bool hasToArribe;

    [SerializeField] Transform initialPoint;

    [SerializeField] Transform endPoint;

    [SerializeField] GameObject platformStructure;

    [SerializeField] LayerMask layerMask;

    Lever[] lstLevers;

    Lever activeLever;

    bool moving = false;

    Vector2 initialPos;

    Vector2 endPos;

    Vector2 start;

    Vector2 end;
    
    // Start is called before the first frame update
    void Start()
    {
        platformStructure.transform.position = new Vector3(platformStructure.transform.position.x, platformStructure.transform.position.y,0);
        initialPos = new Vector2(initialPoint.position.x, initialPoint.position.y);
        endPos = new Vector2(endPoint.position.x, endPoint.position.y);
        lstLevers = GetComponentsInChildren<Lever>();
    }

    void FixedUpdate(){
        if (moving) {
            //moving structure to end pos
            Vector2 structurePos = new Vector2(platformStructure.transform.position.x,platformStructure.transform.position.y);
            platformStructure.transform.position = Vector2.MoveTowards(structurePos, end, CONST_SPEED * Time.deltaTime);

            if (structurePos == end) { 
                //if we have to go back to initial state and we're not
                if (goToInitialState && initialPos != structurePos) 
                {
                    StartCoroutine(IGoToInitialPosition(structurePos));
                } else { //end
                    moving = false;
                    if (activeLever){
                        activeLever.ChangeSprite();
                        activeLever = null;
                    }
                }
            }
        }
    }

    //pre: --
    //post: waits CONST_WAIT_TIME and prepares the variables to move the strucuture back
    IEnumerator IGoToInitialPosition(Vector2 structurePos){
        yield return new WaitForSeconds(CONST_WAIT_TIME);
        start = structurePos == initialPos ? initialPos : endPos;
        end = start == initialPos ? endPos : initialPos;
    }

    //pre: -
    //post: prepares the variables to move the strucuture 
    private void PrepareActivation(){
        start = new Vector2(platformStructure.transform.position.x,platformStructure.transform.position.y) == initialPos ? initialPos : endPos;
        end = start == initialPos ? endPos : initialPos;
        moving = true;
    }

    //pre: -
    //post: prepares the variables to move the strucuture 
    private void Activate(Vector2 posToGo){
        end = posToGo == endPos ? endPos : initialPos;
        start = end == initialPos ? endPos : initialPos;
        moving = true;
    }

    //pre: -
    //post: if collider is player and structure is stoped it activates mechanism
    public void ActivateMechanism(Collider2D other){
        if (((1<<other.gameObject.layer) & layerMask) != 0 && !moving){
            PrepareActivation();
        }
    } 

    //pre: --
    //post: returs true is structure is not in toggle point
    // not moving  OR  structure is moving but hasToArribe is not activated and togglePoint is not the end
    public bool CanActivate(Transform togglePoint){
        return 
        platformStructure.transform.position != togglePoint.position 
        && (!moving || 
        (moving && !hasToArribe && end != new Vector2(togglePoint.position.x,togglePoint.position.y)));
    }

    public void Activate(Lever l){
        activeLever = l;
        Vector2 togglePos = new Vector2(activeLever.GetPoint().position.x,  activeLever.GetPoint().position.y);
        Activate(togglePos);
    }

    public bool CanActivate(Lever l){ 
        Transform togglePoint = l.GetPoint();
        return platformStructure.transform.position != togglePoint.position 
        && (!moving || 
        (moving && !hasToArribe && end != new Vector2(togglePoint.position.x,togglePoint.position.y)));
    }
}
