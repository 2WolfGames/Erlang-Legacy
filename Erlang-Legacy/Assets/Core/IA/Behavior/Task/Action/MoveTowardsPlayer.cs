using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class MoveTowardsPlayer : EnemyAction
    {
        public SharedFloat linerSpeed = 3f;
        public SharedFloat threshold = 0.05f;

        public override TaskStatus OnUpdate()
        {
            MoveTowards();
            return InsideTreshold() ? TaskStatus.Success : TaskStatus.Running;
        }

        private void MoveTowards()
        {
            Transform playerTransform = player.transform;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, linerSpeed.Value * Time.deltaTime);
        }

        private bool InsideTreshold()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= threshold.Value;
        }
    }
}