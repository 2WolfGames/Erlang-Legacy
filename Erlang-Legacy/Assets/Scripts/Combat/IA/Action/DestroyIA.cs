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
        [SerializeField] bool spawnDeadBody;
        private bool destroyTaskCompleted;
        private DeadBodySpawner deadBodySpawner;

        public override void OnAwake()
        {
            deadBodySpawner = GetComponent<DeadBodySpawner>();
            base.OnAwake();
        }

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

                if (spawnDeadBody)
                {
                    SharedEnum.Face facing = transform.localScale.x < 0 ? SharedEnum.Face.Left : SharedEnum.Face.Right;
                    deadBodySpawner?.Spawn(transform.position, facing, Vector2.right * 10 * Vector2.up);
                }

                destroyTaskCompleted = true;
                Object.Destroy(gameObject);
            });
        }

        public override TaskStatus OnUpdate()
        {
            return destroyTaskCompleted ? TaskStatus.Success : TaskStatus.Running;
        }

    }
}
