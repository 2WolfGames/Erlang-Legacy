using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.Combat.IA.Action
{
    public class Wheel : EnemyAction
    {
        // description:
        //      thought to make rotate enemy render 
        [SerializeField] float angularSpeed;
        [SerializeField] Transform target;
        [SerializeField] bool wheelOverPlayer;

        float dir = 1f;

        public override void OnStart()
        {
            if (wheelOverPlayer) dir = ComputeDir();
        }

        public override TaskStatus OnUpdate()
        {
            if (target == null) return TaskStatus.Failure;
            WheelStep(angularSpeed * Time.deltaTime * dir);
            return TaskStatus.Running;
        }

        private void WheelStep(float step)
        {
            target.Rotate(new Vector3(0, 0, step), Space.Self);
        }

        private float ComputeDir()
        {
            return player.transform.position.x > transform.position.x ? -1 : 1;
        }
    }
}

