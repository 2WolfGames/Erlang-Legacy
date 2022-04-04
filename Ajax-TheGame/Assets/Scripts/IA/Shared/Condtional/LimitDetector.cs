using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// description:
//      usufull to know when an enemy has arrive
//      to platform limit or is touching wall
namespace Core.IA.Shared.Conditional
{
    public class LimitDetector : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        [SerializeField] LayerMask whatIsGround;
        [SerializeField] Transform wallChecker;
        [SerializeField] Transform groundChecker;
        [SerializeField] float circleRadius;
        [SerializeField] bool touchingWall;
        [SerializeField] bool touchingGround = true;


        public override TaskStatus OnUpdate()
        {
            touchingGround = Physics2D.OverlapCircle(groundChecker.position, circleRadius, whatIsGround);
            touchingWall = Physics2D.OverlapCircle(wallChecker.position, circleRadius, whatIsGround);
            return (!touchingGround || touchingWall) ? TaskStatus.Success : TaskStatus.Failure;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundChecker.position, circleRadius);
            Gizmos.DrawWireSphere(wallChecker.position, circleRadius);
        }
    }
}

