using BehaviorDesigner.Runtime.Tasks;


// description:
//      scales current x object scale for it's inverse
namespace Core.Combat.IA.Action
{
    public class Scale : BehaviorDesigner.Runtime.Tasks.Action
    {
        public override TaskStatus OnUpdate()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            return TaskStatus.Success;
        }
    }
}