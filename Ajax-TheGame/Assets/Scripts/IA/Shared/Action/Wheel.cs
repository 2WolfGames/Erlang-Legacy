using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


// desc: moves rotating to target
public class Wheel : Action
{
    [SerializeField] bool clockwise;
    [SerializeField] float linearSpeed;
    [SerializeField] float angularSpeed;

    public override TaskStatus OnUpdate()
    {
        var angularStep = clockwise ? -1 : 1 * angularSpeed * Time.deltaTime;
        var linearStep = linearSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, angularStep), Space.Self);
        return TaskStatus.Running;
    }

}
