using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Bahavior.SharedVariable;
using Core.Manager;
using DG.Tweening;
using UnityEngine;

using SharedEnum = Core.Shared.Enum;

namespace Core.Combat.IA.Action
{
    public class DestroyIA : EnemyAction
    {
        public SharedParticleSystem bleedEffect;
        public SharedFloat bleedDuration;
        public SharedParticleSystem explosionEffect;
        public SharedSpriteRenderer deadBody;
        [SerializeField] bool destroyGameObject = true;

        private bool completed = false;

        public override void OnStart()
        {
            if (bleedEffect.Value)
            {
                EffectManager.Instance?.PlayOneShot(bleedEffect.Value, transform);
            }
            DOVirtual.DelayedCall(bleedDuration.Value, KillEnemy);
        }

        public override TaskStatus OnUpdate()
        {
            return completed ? TaskStatus.Success : TaskStatus.Running;
        }

        private void SpawnDeadBody()
        {
            var facing = transform.localScale.x < 0 ? SharedEnum.Face.Left : SharedEnum.Face.Right;
            CorpsesManager.Instance?.Spawn(deadBody.Value, transform.position, facing);
        }

        private void KillEnemy()
        {
            if (explosionEffect.Value)
            {
                EffectManager.Instance?.PlayOneShot(explosionEffect.Value, transform);
            }

            if (deadBody.Value)
            {
                SpawnDeadBody();
            }
            completed = true;
            if (destroyGameObject) Object.Destroy(gameObject);
        }
    }
}
