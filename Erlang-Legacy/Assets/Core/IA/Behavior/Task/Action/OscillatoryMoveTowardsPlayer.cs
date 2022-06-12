using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class OscillatoryMoveTowardsPlayer : EnemyAction
    {
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float linerSpeed = 3f;

        public override TaskStatus OnUpdate()
        {
            MoveTowards();
            return TaskStatus.Success;
        }

        private void MoveTowards()
        {
            Transform playerTransform = player.transform;
            var dir = playerTransform.position - transform.position;
            transform.up = Vector3.MoveTowards(transform.up, dir, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, linerSpeed * Time.deltaTime);
        }
    }
}