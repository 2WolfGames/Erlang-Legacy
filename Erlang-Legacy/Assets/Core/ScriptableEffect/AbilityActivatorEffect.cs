using Core.Player.Controller;
using UnityEngine;
using static Core.Player.Controller.AbilityController;

namespace Core.ScriptableEffect
{
    [CreateAssetMenu(menuName = "Effect/AbilityActivator")]
    public class AbilityActivatorEffect : Effect
    {
        public Ability ability;

        public override void Apply(GameObject other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player == null)
            {
                Debug.LogWarning("DashActivator: Player not found");
                return;
            }
            player.AdquireAbility(ability);
        }
    }
}
