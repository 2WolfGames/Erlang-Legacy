

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Core.IA.Task.Action
{
    public class GoToNextStage : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedInt CurrentStage;
        public override TaskStatus OnUpdate()
        {
            CurrentStage.Value += 1;
            return TaskStatus.Success;
        }
    }
}
