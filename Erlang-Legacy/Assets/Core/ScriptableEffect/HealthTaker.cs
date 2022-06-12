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
            Debug.Log($"{self.name} is taking {amount} damage from {other.name}");
            var player = other.GetComponent<PlayerController>();
            player?.Hurt(amount, self);
        }
    }
}
