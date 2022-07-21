using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.Projectile;
using DG.Tweening;
using UnityEngine;

namespace Core.Combat.IA.Action
{
    public class SpawnFallingRocks : EnemyAction
    {
        [SerializeField] Collider2D spawnArea;
        [SerializeField] AbstractProjectile rockPrefab;
        [SerializeField] int spawnCount = 4;
        [SerializeField] float spawnInterval = 0.3f;
        [SerializeField] bool useRandom = false;
        [SerializeField] int minSpawnCount = 1;
        [SerializeField] int maxSpawnCount = 4;

        public override void OnStart()
        {
            int spawnCount = GetSpawnCount();
            var sequence = DOTween.Sequence();
            for (int i = 0; i < spawnCount; i++)
            {
                sequence.AppendCallback(SpawnRock);
                sequence.AppendInterval(spawnInterval);
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        private void SpawnRock()
        {
            var randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var rock = Object.Instantiate(rockPrefab, new Vector2(randomX, spawnArea.bounds.min.y), Quaternion.identity);
            rock.SetForce(Vector2.zero);
        }

        private int GetSpawnCount()
        {
            return useRandom ? Random.Range(minSpawnCount, maxSpawnCount + 1) : spawnCount;
        }
    }

}
