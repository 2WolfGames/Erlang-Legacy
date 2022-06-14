using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class OscillatoryMoveTowardsPlayer : EnemyAction
    {
        // public SharedFloat rotationSpeed = 5f;
        public SharedFloat speed = 3f;
        public SharedFloat threshold = 0.005f;

        public override TaskStatus OnUpdate()
        {
            MoveTowards();
            return InsideTreshold() ? TaskStatus.Success : TaskStatus.Running;
        }

        private void MoveTowards()
        {
            Transform playerTransform = player.transform;
            var norm = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.Lerp(transform.position, transform.position + norm + new Vector3(norm.x * Time.deltaTime, 0), speed.Value * Time.deltaTime);
        }

        private bool InsideTreshold()
        {
            return Vector2.Distance(transform.position, player.transform.position) <= threshold.Value;
        }
    }
}