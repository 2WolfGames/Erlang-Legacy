using BehaviorDesigner.Runtime.Tasks;
using Core.Utility;
using DG.Tweening;
using UnityEngine;

using SharedEnum = Core.Shared.Enum;

namespace Core.Combat.IA.Action
{
    public class DestroyIA : EnemyAction
    {
        [SerializeField] ParticleSystem bleedEffect;
        [SerializeField] float bleedDuration;
        [SerializeField] ParticleSystem explosionEffect;
        [SerializeField] SpriteRenderer deadBody;

        private bool completed = false;

        public override void OnStart()
        {
            if (bleedEffect)
            {
                EffectManager.Instance?.PlayOneShot(bleedEffect, transform.position);
            }

            DOVirtual.DelayedCall(bleedDuration, () =>
            {
                if (explosionEffect)
                {
                    EffectManager.Instance?.PlayOneShot(explosionEffect, transform.position);
                }

                if (deadBody)
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
            DeadBodiesManager.Instance?.Spawn(deadBody, transform.position, facing);
        }
    }
}
