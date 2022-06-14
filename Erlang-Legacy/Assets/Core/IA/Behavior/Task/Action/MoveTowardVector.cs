using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class MoveTowardVector : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedFloat linerSpeed = 3f;

        public SharedVector3 vector = Vector3.up;

        public override TaskStatus OnUpdate()
        {
            MoveTowards();
            return TaskStatus.Success;
        }

        private void MoveTowards()
        {
            Vector3 dir = transform.position + vector.Value.normalized;
            transform.position = Vector3.MoveTowards(transform.position, dir, linerSpeed.Value * Time.deltaTime);
        }

    }
}