using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.Combat.IA.Action
{
    // description: moves current IA to point
    public class Guard : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] Transform target;
        [SerializeField] bool horizontal = true;
        [SerializeField] float speed;

        Vector2 limit;

        public override TaskStatus OnUpdate()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, ComputeLimit(), step);
            float distance = Vector2.Distance(transform.position, ComputeLimit());
            return distance <= Mathf.Epsilon ? TaskStatus.Success : TaskStatus.Running;
        }

        private Vector2 ComputeLimit()
        {
            if (horizontal)
                return new Vector2(target.position.x, transform.position.y);
            else return target.position;
        }

    }
}
