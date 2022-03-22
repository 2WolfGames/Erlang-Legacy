using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Face : Action
{
    [SerializeField] bool right;

    public override void OnStart()
    {
        _Face();
    }

    private void _Face()
    {
        transform.localScale = new Vector3(right ? 1 : -1, transform.localScale.y, transform.localScale.z);
        Debug.Log(transform.localScale);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}
