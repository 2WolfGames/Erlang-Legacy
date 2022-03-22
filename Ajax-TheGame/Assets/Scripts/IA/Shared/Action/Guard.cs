using System.Collections;
using System.Collections.Generic;
using Core.IA.Shared;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Core.IA.Task
{
    public class Guard : Action
    {
        [SerializeField] Transform limit, other;
        [SerializeField] float period = 10f;

        bool moving, touchesLimit = false;

        public override void OnStart()
        {
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
            _Guard();
        }
        public override TaskStatus OnUpdate()
        {
            return touchesLimit ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            moving = false;
            touchesLimit = false;
        }

        private void _Guard()
        {
            if (moving) return;
            moving = true;
            transform
                .DOMoveX(limit.position.x, ComputeDoMoveXDuration())
                .SetEase(Ease.Linear)
                .OnComplete(() => touchesLimit = true);
        }

        // pre: one limit picked
        private float ComputeDoMoveXDuration()
        {
            // IA should move period/2
            float distance = Mathf.Abs(limit.position.x - transform.position.x);
            float completeDistance = Mathf.Abs(limit.position.x - other.position.x);
            if (completeDistance == 0) return period / 2;
            else return (distance / completeDistance) * (period / 2);
        }
    }
}
