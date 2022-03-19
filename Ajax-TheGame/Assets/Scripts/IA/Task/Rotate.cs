using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime.Tasks;

namespace Core.IA.Task
{
    public class Rotate : Action
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
