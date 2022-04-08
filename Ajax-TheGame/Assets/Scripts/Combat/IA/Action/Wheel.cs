using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Action
{
    public class Wheel : BehaviorDesigner.Runtime.Tasks.Action
    {
        // description:
        //      thought to make rotate enemy render 
        [SerializeField] float angularSpeed;
        [SerializeField] Transform target;

        public override TaskStatus OnUpdate()
        {
            if (target == null) return TaskStatus.Failure;
            WheelStep(angularSpeed * Time.deltaTime);
            return TaskStatus.Running;
        }

        private void WheelStep(float step)
        {
            target.Rotate(new Vector3(0, 0, step), Space.Self);
        }
    }
}

