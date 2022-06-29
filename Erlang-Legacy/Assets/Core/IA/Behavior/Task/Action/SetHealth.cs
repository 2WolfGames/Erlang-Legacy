
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;

namespace Core.IA.Task.Action
{
    public class SetHealth : EnemyAction
    {
        public SharedInt health = 20;
        public override TaskStatus OnUpdate()
        {
            destroyable.CurrentHealth = health.Value;
            return TaskStatus.Success;
        }
    }
}