using System.Collections;
using System.Collections.Generic;
using Core.IA.Shared;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Core.IA.Task
{
    public class Guard : EnemyAction
    {
        [SerializeField] Transform limitA, limitB;
        [SerializeField] float period = 10f;
        [SerializeField] float distanceDetection = 2f;
        [SerializeField] LayerMask whatIsPlayer;

        Transform target;
        bool picked;

        public override void OnStart()
        {
            picked = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (CanSeePlayer()) return TaskStatus.Success;
            Move();
            return TaskStatus.Running;
        }

        private void Move()
        {
            if (picked) return;

            picked = true;

            if (target == null) PickSideRandomly();
            else PickOtherSide();

            transform
                .DOMoveX(target.position.x, period / 2)
                .SetEase(Ease.Linear)
                .OnComplete(() => picked = false);
        }

        private void PickSideRandomly()
        {
            int x = Random.Range(0, 2);
            target = x == 0 ? limitA : limitB;
        }

        private void PickOtherSide()
        {
            if (target.position.x >= limitB.position.x)
            {
                target = limitA;
            }
            else if (target.position.x <= limitA.position.x)
            {
                target = limitB;
            }
        }

        private bool CanSeePlayer()
        {
            Debug.DrawRay(transform.position, Vector2.right, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, distanceDetection, whatIsPlayer);
            return hit.collider;
        }
    }
}
