using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Behavior
{
    public class RotateTaks : Action
    {
        [SerializeField] float speed = 0.1f;

        public override TaskStatus OnUpdate()
        {
            Debug.Log("On update");

            transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
            return TaskStatus.Running;
        }
    }

}

