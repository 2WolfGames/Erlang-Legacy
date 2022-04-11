using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Combat.IA.Action
{
    // description:
    //      moves current object to target position in y axi
    public class MoveVertically : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] Transform target;
        [SerializeField] float delay = 0;
        [SerializeField] float raiseTime = 0.5f;
        bool complete;
        Tween transitionTween;

        public override void OnStart()
        {
            complete = false;
            DOVirtual.DelayedCall(delay, () =>
            {
                _Raise();
            }, false);
        }

        public override TaskStatus OnUpdate()
        {
            return complete ? TaskStatus.Success : TaskStatus.Running;
        }

        private void _Raise()
        {
            transitionTween = transform
                .DOMoveY(target.position.y, raiseTime, false)
                .SetEase(Ease.InQuad)
                .OnComplete(() => complete = true);
        }

        public override void OnEnd()
        {
            // kills transition at end of cicle
            transitionTween?.Kill();
        }
    }
}
