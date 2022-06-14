using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Conditional
{
    public class AlreadyInPosition : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] Transform target;

        public override TaskStatus OnUpdate()
        {
            float d = Vector2.Distance(transform.position, target.position);
            return d <= Mathf.Epsilon ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
