using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Core.Combat.IA.Action
{
    // desc: checks current player location to face to him
    public class FaceTarget : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTransform target;
        float baseScaleX;

        public override void OnAwake()
        {
            baseScaleX = transform.localScale.x;
        }

        // pre: --
        // post: updates local IA scale in order to see player
        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
                return TaskStatus.Failure;

            var scale = transform.localScale;
            scale.x = transform.position.x > target.Value.position.x ? -baseScaleX : baseScaleX;
            transform.localScale = scale;
            return TaskStatus.Success;
        }
    }
}
