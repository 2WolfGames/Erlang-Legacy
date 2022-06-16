using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using DG.Tweening;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class Timer : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        public SharedFloat delay = 1f;
        private bool complete = false;

        public override void OnStart()
        {
            DOVirtual.DelayedCall(delay.Value, () => complete = true);
        }

        public override TaskStatus OnUpdate()
        {
            return complete ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
