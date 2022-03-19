using System.Collections;
using System.Collections.Generic;
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
            var controller = target.GetComponent<AjaxController>();
            controller.TakeLife(amount);
        }
    }
}
