using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


public class Wheel : Action
{
    [SerializeField] bool clockwise;
    [SerializeField] bool infinite;
    [SerializeField] float angularSpeed;
    [SerializeField] Transform target;
    [SerializeField] float phi;

    float zAngle = 0f;
    float step = 0f;

    public override void OnStart()
    {
        if (target == null)
        {
            target = GetComponent<Transform>();
        }
        zAngle = target.rotation.z;
    }


    public override TaskStatus OnUpdate()
    {
        float gap = target.rotation.z - zAngle;
        if (!infinite && gap >= phi) return TaskStatus.Success;
        step = angularSpeed * (clockwise ? -1 : 1) * Time.deltaTime;
        WheelStep(step);
        return TaskStatus.Running;
    }

    private void WheelStep(float step)
    {
        Debug.Log(step);
        target.Rotate(new Vector3(0, 0, step), Space.Self);
    }

}
