using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// description:
//      task usufull for those enemies that
//      uses collider to detect player
namespace Core.Combat.IA.Conditional
{
    public class PlayerDetected : EnemyConditional
    {
        [SerializeField] Collider2D detector;

        public override TaskStatus OnUpdate()
        {
            var touching = player ? detector.IsTouching(player.GetComponent<Collider2D>()) : false;
            return touching ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}
