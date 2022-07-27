using Core.Player.Controller;
using UnityEngine;


namespace Core.ScriptableEffect
{
    // thought to add to elements in game
    // that removes life to ajax
    [CreateAssetMenu(menuName = "Effect/HealthAdder")]
    public class HealthAdder : Effect
    {
        [SerializeField] int amount = 1;
        public override void Apply(GameObject other)
        {
            var player = other.GetComponent<PlayerController>();
            player?.Heal(amount);
        }
    }
}
