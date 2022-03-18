using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DG.Tweening;
using IA.Shared;

namespace IA.Task
{
    public class Jump : EnemyAction
    {
        [SerializeField] float horizontalForce = 5f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float delayedTime = 1f;
        [SerializeField] float jumpTime = 1f;

        List<Tween> tweens = new List<Tween>();

        bool hasLanded = false;

        public override void OnStart()
        {
            hasLanded = false;
            tweens.Add(
                DOVirtual.DelayedCall(delayedTime, StartJump, false)
            );
            ajaxDetector.Hola();
        }
        public override TaskStatus OnUpdate()
        {
            return hasLanded ? TaskStatus.Success : TaskStatus.Running;
        }

        private void StartJump()
        {
            var direction = ajax.transform.position.x < transform.position.x ? -1 : 1;
            body.AddForce(new Vector2(horizontalForce * direction, jumpForce), ForceMode2D.Impulse);
            tweens.Add(
                DOVirtual.DelayedCall(jumpTime, () => hasLanded = true, false)
            );
        }

        public override void OnEnd()
        {
            foreach (Tween tween in tweens)
            {
                tween?.Kill();
            }
        }

    }
}
