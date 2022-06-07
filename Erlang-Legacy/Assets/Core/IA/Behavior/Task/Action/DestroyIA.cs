using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Bahavior.SharedVariable;
using Core.Utility;
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

        private bool completed = false;

        public override void OnStart()
        {
            if (bleedEffect.Value)
            {
                EffectManager.Instance?.PlayOneShot(bleedEffect.Value, transform.position);
            }

            DOVirtual.DelayedCall(bleedDuration.Value, () =>
            {
                if (explosionEffect.Value)
                {
                    EffectManager.Instance?.PlayOneShot(explosionEffect.Value, transform.position);
                }

                if (deadBody.Value)
                {
                    SpawnDeadBody();
                }

                completed = true;
                Object.Destroy(gameObject);
            });
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
    }
}
