using System.Collections;
using UnityEngine;

public class MechanismBase : MonoBehaviour
{
    const float CONST_SPEED = 3f;
    const float CONST_WAIT_TIME = 3f;
    [SerializeField] bool goToInitialState;
    [SerializeField] Transform initialPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] GameObject structure;
    [SerializeField] LayerMask layerMask;
    bool moving = false;
    Transform start;
    Transform end;
    
    // Start is called before the first frame update
    void Start()
    {
        structure.transform.position = new Vector3(structure.transform.position.x, structure.transform.position.y,0);
        initialPoint.position = new Vector3(initialPoint.position.x, initialPoint.position.y,0);
        endPoint.position = new Vector3(endPoint.position.x, endPoint.position.y,0);
    }

    void FixedUpdate(){
        if (moving) {
            structure.transform.position = Vector2.MoveTowards( new Vector2(structure.transform.position.x,structure.transform.position.y), 
            new Vector2(end.position.x,end.position.y), CONST_SPEED * Time.deltaTime);

            if (structure.transform.position == end.position) {
                if (goToInitialState && initialPoint.position != structure.transform.position)
                {
                    StartCoroutine(IGoToInitialPosition());
                } else {
                    moving = false;
                }
            }
        }
    }

    IEnumerator IGoToInitialPosition(){
        yield return new WaitForSeconds(CONST_WAIT_TIME);
        start = structure.transform.position == initialPoint.position ? initialPoint : endPoint;
        end = start == initialPoint ? endPoint : initialPoint;
    }

    private void Activate(){
        start = structure.transform.position == initialPoint.position ? initialPoint : endPoint;
        end = start == initialPoint ? endPoint : initialPoint;
        moving = true;
    }

    public void ActivateMechanism(Collider2D other){
        if (((1<<other.gameObject.layer) & layerMask) != 0 && !moving){
            Activate();
        }
    }

    public void ActivateMechanism(Transform togglePoint){
        Debug.Log(structure.transform.position + " " + togglePoint.position);
        if (structure.transform.position != togglePoint.position && !moving){
            Activate();
        }
    }

}
