using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using IA.Shared;
using UnityEngine;

namespace IA.Task
{
    public class Jump : EnemyAction
    {
        [SerializeField] float horizontalForce = 5f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpTime = 1f;
        bool hasLanded = false;

        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            return hasLanded ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}
