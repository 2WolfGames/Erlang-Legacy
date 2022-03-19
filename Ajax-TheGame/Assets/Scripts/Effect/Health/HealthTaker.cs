using System.Collections;
using System.Collections.Generic;
using Core.Player;
using UnityEngine;


namespace Core.Effect.Health
{
    // thought to add to elements in game
    // that removes life to ajax
    [CreateAssetMenu(menuName = "Effect/HealthTaker")]
    public class HealthTaker : Effect
    {
        [SerializeField] int amount = 1;
        public override void Apply(GameObject target)
        {
            var controller = target.GetComponent<Controller>();
            controller.TakeLife(amount);
        }
    }
}
