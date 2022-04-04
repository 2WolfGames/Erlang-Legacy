using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared;
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
            var touching = collider.IsTouching(player.GetCollider());
            return touching ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}
