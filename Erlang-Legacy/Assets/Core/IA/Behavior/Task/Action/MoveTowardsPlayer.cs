using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class MoveTowardsPlayer : EnemyAction
    {
        [SerializeField] float linerSpeed = 3f;
        [Range(0.001f, 0.5f)][SerializeField] float threshold = 0.05f;

        public override TaskStatus OnUpdate()
        {
            MoveTowards();
            return InsideTreshold() ? TaskStatus.Success : TaskStatus.Running;
        }

        private void MoveTowards()
        {
            Transform playerTransform = player.transform;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, linerSpeed * Time.deltaTime);
        }

        private bool InsideTreshold()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= threshold;
        }
    }
}