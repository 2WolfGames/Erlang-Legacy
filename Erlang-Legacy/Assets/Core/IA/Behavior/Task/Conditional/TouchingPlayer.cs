using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using Core.IA.Bahavior.SharedVariable;
using UnityEngine;

namespace Core.IA.Behavior.Conditional
{
    public class TouchingPlayer : EnemyConditional
    {
        public SharedCollider2D detector;

        public override TaskStatus OnUpdate()
        {
            return IsTouchingPlayer() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool IsTouchingPlayer()
        {
            return detector.Value.IsTouching(player.GetComponent<Collider2D>());
        }
    }
}
