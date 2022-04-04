﻿using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.IA.Shared.Action
{
    public class Face : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] bool right;

        public override void OnStart()
        {
            _Face();
        }

        private void _Face()
        {
            transform.localScale = new Vector3(right ? 1 : -1, transform.localScale.y, transform.localScale.z);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

    }
}


