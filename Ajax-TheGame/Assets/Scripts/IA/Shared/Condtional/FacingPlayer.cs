using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared;
using UnityEngine;

namespace Core.IA.Shared.Conditional
{
    public class FacingPlayer : EnemyConditional
    {
        public override void OnAwake()
        {
            base.OnAwake();
        }

        // pre: --
        // post: check if current IA orientation
        //       faces to player
        public override TaskStatus OnUpdate()
        {
            var toPlayer = (player.transform.position - transform.position).normalized;
            if (Mathf.Abs(toPlayer.x) > 0)
            {
                if (toPlayer.x * transform.localScale.x > 0) return TaskStatus.Success;
                else return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
    }
}