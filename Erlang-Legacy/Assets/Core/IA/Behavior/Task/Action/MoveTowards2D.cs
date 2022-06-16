

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class MoveTowards2D : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTransform target;
        public SharedFloat speed;
        public SharedFloat threshold;

        public override TaskStatus OnUpdate()
        {
            if (TargetAchieved())
                return TaskStatus.Success;
            transform.position = Vector2.MoveTowards(transform.position, target.Value.position, speed.Value * Time.deltaTime);
            return TaskStatus.Running;
        }

        private bool TargetAchieved()
        {
            return Vector2.Distance(transform.position, target.Value.position) <= threshold.Value;
        }
    }
}