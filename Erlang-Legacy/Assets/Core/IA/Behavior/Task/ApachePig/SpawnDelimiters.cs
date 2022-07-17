using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using Core.Combat.Projectile;
using DG.Tweening;
using UnityEngine;

namespace Core.IA.Behavior.ApachePig
{
    public class SpawnDelimiters : EnemyAction
    {
        [SerializeField] Collider2D delimiterArea;
        [SerializeField] AbstractProjectile delimiterPrefab;
        [SerializeField] float spawnInterval = 0.3f;
        [SerializeField] bool reverse = false;
        private Stack<float> delimitersX = new Stack<float>();
        private int nDelimiters = 2;

        public override void OnStart()
        {
            if (reverse)
            {
                delimitersX.Push(delimiterArea.bounds.min.x);
                delimitersX.Push(delimiterArea.bounds.max.x);
            }
            else
            {
                delimitersX.Push(delimiterArea.bounds.max.x);
                delimitersX.Push(delimiterArea.bounds.min.x);
            }
        }
        public override TaskStatus OnUpdate()
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < nDelimiters; i++)
            {
                sequence.AppendCallback(SpawnDelimiter);
                sequence.AppendInterval(spawnInterval);
            }
            return TaskStatus.Success;
        }

        private void SpawnDelimiter()
        {
            var rock = Object.Instantiate(delimiterPrefab, new Vector2(delimitersX.Pop(), delimiterArea.bounds.min.y), Quaternion.identity);
            rock.SetForce(Vector2.zero);
        }
    }
}
