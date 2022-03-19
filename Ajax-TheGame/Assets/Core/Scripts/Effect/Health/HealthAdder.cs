using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect.Health
{
    // thought to add to elements in game
    // that removes life to ajax
    [CreateAssetMenu(menuName = "Effect/HealthAdder")]
    public class HealthAdderEffect : Effect
    {
        [SerializeField] int amount = 1;
        public override void Apply(GameObject target)
        {
            var controller = target.GetComponent<AjaxController>();
            controller.AddLife(amount);
        }
    }
}
