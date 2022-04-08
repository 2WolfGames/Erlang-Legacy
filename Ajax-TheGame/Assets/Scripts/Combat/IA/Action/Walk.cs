using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// description:
//      moves object with certain speed using 
//      current scale as direction movement

namespace Core.Combat.IA.Action
{
    public class Walk : EnemyAction
    {
        [SerializeField] float speed;

        public override TaskStatus OnUpdate()
        {
            body.velocity = Vector2.right * transform.localScale.x * speed;
            return TaskStatus.Running;
        }
    }
}
