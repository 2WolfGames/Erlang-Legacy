using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

namespace Core.IA.Behavior.Task.Action
{
    public class SetBoolDelayed : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        [Tooltip("The bool value to set")]
        public SharedBool boolValue;
        [Tooltip("The variable to store the result")]
        public SharedBool storeResult;
        public SharedFloat delayBoolSetter = 1f;

        public override void OnStart()
        {
            DOVirtual.DelayedCall(delayBoolSetter.Value, () => storeResult.Value = boolValue.Value);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            boolValue = false;
        }
    }
}
