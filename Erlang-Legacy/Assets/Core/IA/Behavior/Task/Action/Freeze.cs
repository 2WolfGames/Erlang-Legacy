using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.Combat.IA.Behavior.Action
{
    public class Freeze : EnemyAction
    {
        public override void OnStart()
        {
            body.velocity = Vector2.zero;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
