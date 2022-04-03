using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


// description:
//      scales current x object scale for it's inverse
namespace Core.IA.Shared.Action
{
    public class Scale : BehaviorDesigner.Runtime.Tasks.Action
    {
        public override TaskStatus OnUpdate()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            return TaskStatus.Success;
        }
    }
}