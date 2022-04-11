using UnityEngine;

using Func = Core.Shared.Function;

namespace Core.Combat.IA.Jumper
{
    public class RedirectJump : EnemyAction
    {
        [SerializeField] LayerMask whatIsGround;
        [SerializeField][Range(0.1f, 1f)] float softRedirect = 1;

        public override void OnStart()
        {
            if (body.IsTouchingLayers(whatIsGround)) Redirect();
        }

        private void Redirect()
        {

            Debug.Log("redirect!");
            Debug.Log($"current velocity {body.velocity}");
            var vx = Mathf.Abs(body.velocity.x);
            vx = vx == 0 ? 10 : vx;
            var vy = Mathf.Abs(body.velocity.y);
            vy = vy == 0 ? 5 : vy;
            var dx = GetComponent<Collider2D>().bounds.extents.x + 0.3f;
            var dy = GetComponent<Collider2D>().bounds.extents.y + 0.3f;

            var down = Func.Look(transform.position, Vector2.down, dy, whatIsGround, 0.5f);
            var up = Func.Look(transform.position, Vector2.up, dy, whatIsGround, 0.5f);
            var left = Func.Look(transform.position, Vector2.left, dx, whatIsGround, 0.5f);
            var right = Func.Look(transform.position, Vector2.right, dx, whatIsGround, 0.5f);

            body.velocity = Vector2.zero;

            if (up)
            {
                if (left)
                {
                    Debug.Log("1");
                    body.AddForce(new Vector2(vx, -vy) * softRedirect, ForceMode2D.Impulse);
                }
                else if (right)
                {
                    Debug.Log("2");

                    body.AddForce(new Vector2(-vx, -vy) * softRedirect, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.Log("3");
                    body.AddForce(new Vector2(0, -vy) * softRedirect, ForceMode2D.Impulse);
                }

                return;
            }

            if (down)
            {
                if (left)
                {
                    Debug.Log("4");
                    body.AddForce(new Vector2(vx, vy) * softRedirect, ForceMode2D.Impulse);
                }
                else if (right)
                {
                    Debug.Log("5");
                    body.AddForce(new Vector2(-vx, vy) * softRedirect, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.Log("6");
                    body.AddForce(new Vector2(0, vy) * softRedirect, ForceMode2D.Impulse);
                }

                return;
            }

            if (right)
            {
                Debug.Log("7");

                body.AddForce(new Vector2(-vx, 0) * softRedirect, ForceMode2D.Impulse);
                return;
            }

            if (left)
            {
                Debug.Log("8");
                body.AddForce(new Vector2(vx, 0) * softRedirect, ForceMode2D.Impulse);
                return;
            }

            Debug.Log($"redirect velocity {body.velocity}");
        }
    }

}
