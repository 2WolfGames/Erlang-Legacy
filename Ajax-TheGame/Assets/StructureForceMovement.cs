using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureForceMovement : MonoBehaviour
{
    [SerializeField] GameObject structure;
    [SerializeField] float speed;
    [SerializeField] MovementPlatform movementPlatformEngine;
    Transform targetPoint;



    private void FixedUpdate() {
        if (targetPoint){
            var currentPos = structure.transform.position;

            structure.transform.position = Vector2.MoveTowards(currentPos, targetPoint.position, speed * Time.deltaTime);

            if (structure.transform.position == targetPoint.position)
            {
                targetPoint = null;
                movementPlatformEngine.enabled = true;
            }
        }
    }

    public void Activate(Transform pointToGo){
        this.targetPoint = pointToGo;
        movementPlatformEngine.enabled = false;
        movementPlatformEngine.target = null;
    }
}
