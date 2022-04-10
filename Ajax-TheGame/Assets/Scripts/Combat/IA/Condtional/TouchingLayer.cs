using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Conditional
{
    // description:
    //  conditional task to know if two layers are colliding
    public class TouchingLayer : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        [SerializeField] LayerMask layerMask;

        public override TaskStatus OnUpdate()
        {
            var touching = GetComponent<Collider2D>().IsTouchingLayers(layerMask);
            return touching ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}
