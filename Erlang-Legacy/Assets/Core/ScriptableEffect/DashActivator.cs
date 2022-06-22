using Core.Player.Controller;
using UnityEngine;
using static Core.Player.Controller.AbilityController;

namespace Core.ScriptableEffect
{
    public class AbilityActivator : Effect
    {
        public Skill newSkill;
        public override void Apply(GameObject other)
        {
            var player = other.GetComponent<PlayerController>();
            if (!player)
            {
                Debug.LogWarning("DashActivator: Player not found");
                return;
            }
            player.ActiveSkill(newSkill);
        }
    }
}
