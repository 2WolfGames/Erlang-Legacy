
using BehaviorDesigner.Runtime.Tasks;
using Core.Shared;
using UnityEngine;

// description:
//  task to make IA wait for down grounded collision
namespace Core.Combat.IA.Conditional
{
    public class WaitForGrounded : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] LayerMask whatIsGround;

        public override TaskStatus OnUpdate()
        {
            var grounded = Grounded;
            return grounded ? TaskStatus.Success : TaskStatus.Running;
        }

        private bool Grounded
        {
            get
            {
                float distance = Function.VerticalExtentsDimention(GetComponent<Collider2D>()) + 0.1f;
                return Function.Look(transform.position, Vector2.down, distance, whatIsGround, 0.5f);
            }
        }
    }

}
