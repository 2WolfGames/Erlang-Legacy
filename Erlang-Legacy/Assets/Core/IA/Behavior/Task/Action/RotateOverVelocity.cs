
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Action
{
    // description: rotate object over current velocity
    public class RotateOverVelocity : EnemyAction
    {
        [SerializeField] float angularSpeed;
        [SerializeField] Transform render;

        public override TaskStatus OnUpdate()
        {
            WheelStep(angularSpeed * Time.deltaTime * body.velocity.x >= 0 ? -1 : 1);
            return TaskStatus.Running;
        }

        private void WheelStep(float step)
        {
            render.Rotate(new Vector3(0, 0, step), Space.Self);
        }

    }

}
