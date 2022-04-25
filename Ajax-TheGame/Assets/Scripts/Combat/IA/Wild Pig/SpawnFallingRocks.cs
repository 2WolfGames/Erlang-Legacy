using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.Projectile;
using DG.Tweening;
using UnityEngine;

namespace Core.Combat.IA.WildPig
{
    public class SpawnFallingRocks : EnemyAction
    {
        [SerializeField] Collider2D spawnArea;
        [SerializeField] AbstractProjectile rockPrefab;
        [SerializeField] int spawnCount = 4;
        [SerializeField] float spawnInterval = 0.3f;

        public override TaskStatus OnUpdate()
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < spawnCount; i++)
            {
                sequence.AppendCallback(SpawnRock);
                sequence.AppendInterval(spawnInterval);
            }
            return TaskStatus.Success;
        }

        private void SpawnRock()
        {
            var randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var rock = Object.Instantiate(rockPrefab, new Vector2(randomX, spawnArea.bounds.min.y), Quaternion.identity);
            rock.SetForce(Vector2.zero);
        }
    }

}
