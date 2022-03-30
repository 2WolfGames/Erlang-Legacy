﻿using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Shared.Action;
using DG.Tweening;
using UnityEngine;

namespace Core.IA.Shared.Action
{
    public class JumpOverPlayer : EnemyAction
    {
        [SerializeField][Range(1f, 10f)] float height = 4f;
        [SerializeField][Range(0.01f, 0.5f)] float threshold = 0.05f;
        [SerializeField][Range(1, 4)] int soft = 2;

        Vector2 start;
        Vector2 end;
        float t;

        public override void OnStart()
        {
            t = 0;
            start = transform.position;
            end = player.Feets.position;
        }

        public override TaskStatus OnUpdate()
        {
            if (ParabolaCompletes()) return TaskStatus.Success;
            t += Time.deltaTime;
            transform.position = MathParabola.Parabola(start, end, height, t / soft);
            return TaskStatus.Running;
        }

        private bool ParabolaCompletes()
        {
            return t >= 0.05f && IsGrounded();
        }

    }
}
