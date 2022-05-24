using BehaviorDesigner.Runtime.Tasks;
using Core.Utility;
using DG.Tweening;
using UnityEngine;

using SharedEnum = Core.Shared.Enum;

namespace Core.Combat.IA.Action
{
    public class DestroyIA : EnemyAction
    {
        [SerializeField] ParticleSystem exploteEffect;
        [SerializeField] ParticleSystem bleedEffect;
        [SerializeField] float bleedDuration;
        [SerializeField] SpriteRenderer deadBody;
        private bool destroyTaskCompleted;

        public override void OnStart()
        {
            if (bleedEffect)
            {
                EffectManager.Instance?.PlayOneShot(bleedEffect, transform.position);
            }

            DOVirtual.DelayedCall(bleedDuration, () =>
            {
                if (exploteEffect)
                {
                    EffectManager.Instance?.PlayOneShot(exploteEffect, transform.position);
                }

                if (deadBody)
                {
                    SpawnDeadBody();
                }

                destroyTaskCompleted = true;
                Object.Destroy(gameObject);
            });
        }

        public override TaskStatus OnUpdate()
        {
            return destroyTaskCompleted ? TaskStatus.Success : TaskStatus.Running;
        }

        private void SpawnDeadBody()
        {
            var facing = transform.localScale.x < 0 ? SharedEnum.Face.Left : SharedEnum.Face.Right;
            DeadBodiesManager.Instance?.Spawn(deadBody, transform.position, facing, Vector2.up * ComputeScaleForce());
        }

        private float ComputeScaleForce()
        {
            return 10f;
        }
    }
}
