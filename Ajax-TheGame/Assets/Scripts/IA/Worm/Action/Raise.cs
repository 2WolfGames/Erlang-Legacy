using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

using Core.IA.Shared.Action;
using BehaviorDesigner.Runtime.Tasks;


namespace Core.IA.Worm.Action
{
    public class Raise : EnemyAction
    {
        [SerializeField] Transform target;
        [SerializeField] float delay = 0;
        [SerializeField] float speed = 0.5f;
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
                .DOMoveY(target.position.y, speed, false)
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
