using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;

namespace Core.IA.Task.Conditional
{
    public class IsHealthUnder : EnemyConditional
    {
        public SharedInt healthThreshold;

        public override TaskStatus OnUpdate()
        {
            return destroyable.CurrentHealth < healthThreshold.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}