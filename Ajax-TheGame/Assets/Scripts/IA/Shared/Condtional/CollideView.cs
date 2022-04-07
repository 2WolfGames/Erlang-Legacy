using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// description:
//      task usufull for those enemies that
//      uses collider to detect player
namespace Core.IA.Shared.Conditional
{
    public class CollideView : EnemyConditional
    {
        [SerializeField] Collider2D collider;

        public override TaskStatus OnUpdate()
        {
            var touching = player ? collider.IsTouching(player.GetComponent<Collider2D>()) : false;
            return touching ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}
