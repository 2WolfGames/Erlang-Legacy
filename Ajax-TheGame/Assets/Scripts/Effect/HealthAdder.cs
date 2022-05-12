using Core.Player.Controller;
using Core.Shared;
using UnityEngine;


namespace Core.Effect
{
    [CreateAssetMenu(menuName = "Effect/HealthAdder")]
    public class HealthAdderEffect : Effect
    {
        [SerializeField] int amount = 1;
        public override void Apply(GameObject other)
        {
            var player = other.GetComponent<PlayerController>();
            player?.Heal(amount);
        }
    }
}
