using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Conditional
{
    // description:
    //  conditional task to know if two layers are colliding
    public class TouchingLayer : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        [UnityEngine.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [SerializeField] LayerMask layerMask;

        private Collider2D collider2D;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                collider2D = currentGameObject.GetComponent<Collider2D>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            var touching = collider2D.IsTouchingLayers(layerMask);
            return touching ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
