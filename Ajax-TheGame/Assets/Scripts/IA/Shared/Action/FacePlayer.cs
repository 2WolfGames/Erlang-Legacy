using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared.Action;

// Checks current player location to face to him
namespace Core.IA.Shared.Action
{
    public class FacePlayer : EnemyAction
    {
        float baseScaleX;

        public override void OnAwake()
        {
            base.OnAwake();
            baseScaleX = transform.localScale.x;
        }

        // pre: --
        // post: updates local IA scale in order to see player
        public override TaskStatus OnUpdate()
        {
            var scale = transform.localScale;
            scale.x = transform.position.x > player.transform.position.x ? -baseScaleX : baseScaleX;
            transform.localScale = scale;
            return TaskStatus.Success;
        }
    }
}
