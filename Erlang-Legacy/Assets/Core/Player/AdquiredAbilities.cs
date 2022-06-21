using UnityEngine;
using static Core.Player.Controller.AbilityController;

namespace Core.Player
{
    // desacoples logic from Ajax instance in every scene
    [CreateAssetMenu(menuName = "Player/AdquiredAbilities")]
    public class AdquiredAbilities : ScriptableObject
    {
        public bool dash;
        public bool ray;

        public bool Adquired(Ability ability)
        {
            switch (ability)
            {
                case Ability.Dash:
                    return dash;
                case Ability.Ray:
                    return ray;
                default:
                    Debug.LogWarning($"Unsupported ability {ability}");
                    return false;
            }
        }
    }
}
