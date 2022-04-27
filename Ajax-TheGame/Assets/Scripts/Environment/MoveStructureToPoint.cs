using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStructureToPoint : MonoBehaviour
{
    [SerializeField] GameObject structure;
    [SerializeField] float speed;
    MoveStructure moveStructureEngine;
    Transform target;

    //pre: --
    //post: if target position is not null, every frame structure moves to it. 
    //      if has arribed, we set the target to none and if the object has a MoveStructureEngine
    //      we enable it 
    private void FixedUpdate()
    {
        if (target)
        {
            var currentPos = structure.transform.position;

            structure.transform.position = Vector2.MoveTowards(currentPos, target.position, speed * Time.deltaTime);

            if (structure.transform.position == target.position)
            {
                target = null;
                if (moveStructureEngine)
                {
                    moveStructureEngine.EnableMove(true);
                    moveStructureEngine = null;
                }

            }
        }
    }

    //pre: --
    //post: target is uptaed with new point 
    //      and if game object has a MoveStructureEngine, we disable it. 
    public void Activate(Transform targetPoint)
    {
        this.target = targetPoint;

        moveStructureEngine = GetComponentInParent<MoveStructure>();
        if (moveStructureEngine)
        {
            moveStructureEngine.EnableMove(false);
        }

    }
}
