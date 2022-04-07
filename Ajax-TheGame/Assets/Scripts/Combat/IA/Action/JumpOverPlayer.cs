using BehaviorDesigner.Runtime.Tasks;
using Core.Shared;
using UnityEngine;

namespace Core.Combat.IA.Action
{
    public class JumpOverPlayer : EnemyAction
    {
        [SerializeField][Range(1f, 10f)] float height = 4f;
        [SerializeField][Range(0.1f, 3f)] float soft = 1f;
        [SerializeField] bool limitedRange;
        [SerializeField][Range(1f, 25f)] float maxRange = 5f;
        [SerializeField] LayerMask whatIsGround;

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

        // desc: because of parabole movement
        //      normal ground detection does not work
        //      when ever touches ground it'is need to stop
        //      to avoid get inside ground
        protected bool IsGrounded()
        {
            float distance = Function.VerticalExtentsDimention(GetComponent<Collider2D>()) + 0.1f;
            return Function.Look(transform.position, Vector2.down, distance, whatIsGround, 0.5f);
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
