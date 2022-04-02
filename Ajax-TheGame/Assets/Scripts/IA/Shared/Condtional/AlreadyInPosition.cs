using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

using System.Windows;

namespace Core.IA.Shared.Action
{
    public class AlreadyInPosition : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] Transform target;

        public override TaskStatus OnUpdate()
        {
            float d = Vector2.Distance(transform.position, target.position);
            return d <= Mathf.Epsilon ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
