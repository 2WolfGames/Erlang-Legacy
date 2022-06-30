
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Manager;

namespace Core.IA.Task.Action
{
    public class FreezeTime : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedFloat duration = 1f;
        public override TaskStatus OnUpdate()
        {
            GameManager.Instance?.FreezeTime(duration.Value);
            return TaskStatus.Success;
        }
    }
}