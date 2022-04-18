using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.Combat.IA.Action
{
    // description:
    //  adds force using a log equation depending on height configuration
    public class JumpOverPlayer : EnemyAction
    {
        [SerializeField][Range(1f, 10f)] float height = 4f;
        [SerializeField] LayerMask whatIsGround;
        [SerializeField] SharedBool jumping;

        public override void OnStart()
        {
            Jump();
            StartCoroutine(DetectableJump());
        }

        public override TaskStatus OnUpdate()
        {
            if (jumping.Value && TouchingGround())
            {
                jumping.Value = false;
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        private IEnumerator DetectableJump()
        {
            yield return new WaitForSeconds(0.1f);
            jumping.Value = true;
        }

        private void Jump()
        {
            var distance = Vector2.Distance(transform.position, player.transform.position);
            var direction = transform.position.x > player.transform.position.x ? -1 : 1;
            var vectorModifier = Vector2.right * distance * Mathf.Log(height, 10) * direction;
            body.AddForce(new Vector2(distance * direction, height * body.gravityScale) + vectorModifier, ForceMode2D.Impulse);
        }

        protected bool TouchingGround()
        {
            var collider = GetComponent<Collider2D>();
            return collider.IsTouchingLayers(whatIsGround);
        }
    }
}
