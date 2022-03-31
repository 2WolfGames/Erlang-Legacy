using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared.Action;
using UnityEngine;

public class Walk : EnemyAction
{
    [SerializeField] float speed;

    public override TaskStatus OnUpdate()
    {
        body.velocity = Vector2.right * transform.localScale.x * speed;
        return TaskStatus.Running;
    }
}
