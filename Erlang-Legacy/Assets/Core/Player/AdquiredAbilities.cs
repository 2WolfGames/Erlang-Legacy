using UnityEngine;

namespace Core.Player
{
    // desacoples logic from Ajax instance in every scene
    [CreateAssetMenu(menuName = "Player/AdquiredAbilities")]
    public class AdquiredAbilities : ScriptableObject
    {
        [SerializeField] bool dash;
        [SerializeField] bool ray;

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

        public void Adquire(Ability ability)
        {
            switch (ability)
            {
                case Ability.Dash:
                    dash = true;
                    break;
                case Ability.Ray:
                    ray = true;
                    break;
                default:
                    Debug.LogWarning($"Unsupported ability {ability}");
                    break;
            }
        }
    }
}
