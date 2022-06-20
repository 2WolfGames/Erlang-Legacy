using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class MoveTowardVector2D : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedFloat linerSpeed = 3f;
        public SharedVector2 vector = Vector2.up;

        public override TaskStatus OnUpdate()
        {
            MoveTowards2D();
            return TaskStatus.Success;
        }

        private void MoveTowards2D()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = position + vector.Value.normalized;
            transform.position = Vector2.MoveTowards(transform.position, dir, linerSpeed.Value * Time.deltaTime);
        }
    }
}

