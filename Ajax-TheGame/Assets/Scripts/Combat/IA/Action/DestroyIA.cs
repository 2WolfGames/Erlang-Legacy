using BehaviorDesigner.Runtime.Tasks;
using Core.Utility;
using DG.Tweening;
using UnityEngine;


namespace Core.Combat.IA.Action
{
    public class DestroyIA : EnemyAction
    {
        [SerializeField] ParticleSystem exploteEffect;
        [SerializeField] ParticleSystem bleedEffect;
        [SerializeField] float bleedDuration;

        private bool destroyTaskCompleted;

        public override void OnStart()
        {
            if (bleedEffect)
                EffectManager.Instance?.PlayOneShot(bleedEffect, transform.position);

            DOVirtual.DelayedCall(bleedDuration, () =>
            {
                if (exploteEffect)
                    EffectManager.Instance?.PlayOneShot(exploteEffect, transform.position);
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
