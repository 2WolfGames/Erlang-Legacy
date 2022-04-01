using System.Collections;
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
        [SerializeField][Range(0.1f, 3f)] float soft = 1f;
        [SerializeField] bool limitedRange;
        [SerializeField][Range(1f, 25f)] float maxRange = 5f;

        Vector2 start;
        Vector2 end;
        float t;

        public override void OnStart()
        {
            t = 0;
            start = transform.position;
            if (limitedRange) end = ComputeFallPoint(start, player.Feets.position, maxRange);
            else end = player.Feets.position;
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

        // pre: 
        // post: compute fall point betwen point A & B
        //      if B is inside A + Vector.left * maxRange and A + Vector.right * maxRange
        //      then returns B as target point
        private Vector2 ComputeFallPoint(Vector2 A, Vector2 B, float maxRange)
        {
            if (Mathf.Abs(A.x - B.x) <= maxRange)
            {
                return B;
            }
            else
            {
                if (B.x < A.x)
                {
                    return new Vector2(A.x - maxRange, B.y);
                }
                else
                {
                    return new Vector2(A.x + maxRange, B.y);
                }
            }
        }

    }
}
