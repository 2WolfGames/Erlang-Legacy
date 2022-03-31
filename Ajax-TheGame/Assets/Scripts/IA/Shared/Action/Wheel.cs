using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared.Action;
using UnityEngine;


public class Wheel : EnemyAction
{
    [SerializeField] bool facePlayer;
    [SerializeField] bool rotateOverScale;
    [SerializeField] bool infinite;
    [SerializeField] float angularSpeed;
    [SerializeField] Transform target;
    [SerializeField] float phi;

    float zAngle = 0f;
    float step = 0f;
    [SerializeField] float ori = 1f;

    public override void OnStart()
    {
        if (target == null)
        {
            target = GetComponent<Transform>();
        }
        zAngle = target.rotation.z;
        ComputeRotationDirection();
    }

    public override TaskStatus OnUpdate()
    {
        float gap = target.rotation.z - zAngle;
        if (!infinite && gap >= phi) return TaskStatus.Success;
        ComputeRotationDirection();
        step = angularSpeed * ori * Time.deltaTime;
        WheelStep(step);
        return TaskStatus.Running;
    }

    private void ComputeRotationDirection()
    {
        var face = transform.position.x > player.transform.position.x ? 1 : -1;

        if (target.gameObject == gameObject)
        {
            var xScale = transform.localScale.x;
            ori = facePlayer ? face : (rotateOverScale ? xScale : 1);
        }
        else
        {
            ori = facePlayer ? face : -1;
        }
    }

    private void WheelStep(float step)
    {
        target.Rotate(new Vector3(0, 0, step), Space.Self);
    }

}
