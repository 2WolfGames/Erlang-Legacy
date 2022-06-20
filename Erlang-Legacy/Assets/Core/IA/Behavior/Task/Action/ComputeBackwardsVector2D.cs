
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class ComputeBackwardsVector2D : EnemyAction
    {
        public SharedVector2 vector = Vector2.up;
        public SharedBool fly = false;

        public override void OnStart()
        {
            vector.Value = player.transform.position.x > transform.position.x ? Vector2.left : Vector2.right;
            if (fly.Value)
            {
                vector.Value = new Vector2(vector.Value.x, 0.5f);
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
