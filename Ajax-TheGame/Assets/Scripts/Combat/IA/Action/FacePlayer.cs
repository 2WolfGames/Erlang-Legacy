using BehaviorDesigner.Runtime.Tasks;

namespace Core.Combat.IA.Action
{
    // desc: checks current player location to face to him
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
