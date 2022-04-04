using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

using Core.Shared;

namespace Core.IA.Shared.Conditional
{
    public class PlayerInFront : EnemyConditional
    {
        [SerializeField] Transform origin;
        [SerializeField] LayerMask playerMask;
        [Range(0, 360)][SerializeField] float visualAngle = 160f;
        [SerializeField] float visualAcuity = 3f;
        [Range(1, 20)][SerializeField] int density = 4;

        public override TaskStatus OnUpdate()
        {
            bool ajaxInFront = Function.LookAround(
                origin,
                Vector2.right * transform.localScale.x,
                visualAcuity,
                visualAngle,
                density,
                playerMask);
            return ajaxInFront ? TaskStatus.Success : TaskStatus.Failure;
        }

    }

}
