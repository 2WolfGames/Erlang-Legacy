using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared;
using UnityEngine;


namespace Core.IA.Shared
{
    public class PlayerEnterArea : EnemyConditional
    {
        [SerializeField] Detector detector;

        public override TaskStatus OnUpdate()
        {
            return detector.InsideArea ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
