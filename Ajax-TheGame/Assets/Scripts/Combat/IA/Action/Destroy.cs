﻿using BehaviorDesigner.Runtime.Tasks;
using Core.Util;
using DG.Tweening;
using UnityEngine;


namespace Core.Combat.IA.Action
{
    public class Destroy : EnemyAction
    {
        [SerializeField] ParticleSystem bleedEffect;
        [SerializeField] float bleedDuration;
        [SerializeField] ParticleSystem exploteEffect;

        private bool isDestroyed;

        public override void OnStart()
        {
            if (bleedEffect)
                EffectManager.Instance?.PlayOneShot(bleedEffect, transform.position);

            DOVirtual.DelayedCall(bleedDuration, () =>
            {
                if (exploteEffect)
                    EffectManager.Instance?.PlayOneShot(exploteEffect, transform.position);
                isDestroyed = true;
                Object.Destroy(gameObject);
            });
        }

        public override TaskStatus OnUpdate()
        {
            return isDestroyed ? TaskStatus.Success : TaskStatus.Running;
        }

    }
}
