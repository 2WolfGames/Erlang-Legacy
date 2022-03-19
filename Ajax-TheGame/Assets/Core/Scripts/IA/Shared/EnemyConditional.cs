﻿using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace IA.Shared
{
    public class EnemyConditional : Conditional
    {
        protected Rigidbody2D body;
        protected Animator animator;

        public override void OnAwake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
    }
}

