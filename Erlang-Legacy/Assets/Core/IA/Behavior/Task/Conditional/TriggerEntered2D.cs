using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.IA.Behavior.Task.Conditional
{
    public class TriggerEntered2D : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        private bool entered = false;

        public override TaskStatus OnUpdate()
        {
            return entered ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            entered = true;
        }

        public override void OnReset()
        {
            entered = false;
        }
    }
}
