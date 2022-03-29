﻿using System.Collections;
using System.Collections.Generic;
using Core.Shared;
using UnityEngine;


namespace Core.Effect
{
    // thought to add to elements in game
    // that removes life to ajax
    [CreateAssetMenu(menuName = "Effect/HealthAdder")]
    public class HealthAdderEffect : Effect
    {
        [SerializeField] int amount = 1;
        public override void Apply(GameObject target)
        {
            var controller = target.GetComponent<LifeController>();
            controller?.AddLife(amount);
        }
    }
}