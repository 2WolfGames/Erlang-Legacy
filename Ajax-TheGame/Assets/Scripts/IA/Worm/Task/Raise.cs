using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

using Core.IA.Shared.Action;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Core.IA.Worm.Action
{
    public class Raise : EnemyAction
    {
        [SerializeField] Transform target;
        [SerializeField] float delay = 0;
        [SerializeField] float speed = 0.5f;
        [SerializeField] SharedBool insideArea;

        public override void OnAwake()
        {
            Debug.Log(insideArea);

            DOVirtual.DelayedCall(delay, () =>
            {
                _Raise();
            }, false);
        }

        public override TaskStatus OnUpdate()
        {
            insideArea.Value = true;
            Debug.Log(insideArea);
            return TaskStatus.Running;
        }

        private void _Raise()
        {
            transform.DOMoveY(target.position.y, speed, false).SetEase(Ease.InQuad);
        }
    }
}
