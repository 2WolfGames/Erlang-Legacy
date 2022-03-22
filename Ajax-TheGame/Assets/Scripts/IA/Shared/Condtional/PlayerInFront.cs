using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared;
using UnityEngine;

namespace Core.IA.Shared.Conditional
{
    public class PlayerInFront : EnemyConditional
    {
        [SerializeField] float rayLenght = 1f;

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

    }

}
