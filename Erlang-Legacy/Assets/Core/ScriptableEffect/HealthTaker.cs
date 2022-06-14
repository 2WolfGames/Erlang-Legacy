using Core.Player.Controller;
using UnityEngine;


namespace Core.ScriptableEffect
{
    // thought to add to elements in game
    // that removes life to ajax
    [CreateAssetMenu(menuName = "Effect/HealthTaker")]
    public class HealthTaker : Effect
    {
        [SerializeField] int amount = 1;
        public override void Apply(GameObject self, GameObject other)
        {
            var player = other.GetComponent<PlayerController>();
            player?.Hurt(amount, self);
        }
    }
}
