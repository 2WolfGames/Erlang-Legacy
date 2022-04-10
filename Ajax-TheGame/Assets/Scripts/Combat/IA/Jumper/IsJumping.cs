using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Jumper
{
    public class IsJumping : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        [SerializeField] SharedBool jumping;

        public override TaskStatus OnUpdate()
        {
            return jumping.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}
