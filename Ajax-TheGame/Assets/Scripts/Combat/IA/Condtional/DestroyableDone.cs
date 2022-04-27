using BehaviorDesigner.Runtime.Tasks;


namespace Core.Combat.IA.Conditional
{
    public class DestroyableDone : EnemyConditional
    {
        public override TaskStatus OnUpdate()
        {
            bool destroyed = destroyable.IsDestroyed;
            return destroyed ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
