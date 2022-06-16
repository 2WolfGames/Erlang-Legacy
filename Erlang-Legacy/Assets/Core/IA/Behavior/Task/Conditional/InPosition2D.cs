

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class InPosition2D : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        public SharedTransform target;
        public SharedFloat threshold;

        public override TaskStatus OnUpdate()
        {
            return TargetAchieved() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool TargetAchieved()
        {
            return Vector2.Distance(transform.position, target.Value.position) <= threshold.Value;
        }
    }
}