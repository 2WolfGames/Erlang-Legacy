using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared.Action;
using UnityEngine;

// description:
//      moves object with certain speed using 
//      current scale as direction movement

namespace Core.IA.Shared.Action
{
    public class Walk : EnemyAction
    {
        [SerializeField] float speed;

        public override TaskStatus OnUpdate()
        {
            body.velocity = Vector2.right * transform.localScale.x * speed;
            return TaskStatus.Running;
        }
    }
}
