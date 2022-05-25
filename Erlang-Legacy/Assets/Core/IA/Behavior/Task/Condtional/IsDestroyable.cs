using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.Combat.IA.Conditional
{
    public class IsDestroyable : EnemyConditional
    {
        public SharedString hola;

        public override TaskStatus OnUpdate()
        {
            Debug.Log(hola.Value);
            bool destroyed = destroyable.IsDestroyed;
            return destroyed ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
