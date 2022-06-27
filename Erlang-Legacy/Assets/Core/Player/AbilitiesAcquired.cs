using UnityEngine;

namespace Core.Player
{
    // desacoples logic from Ajax instance in every scene
    [CreateAssetMenu(menuName = "Player/AbilitiesAcquired")]
    public class AbilitiesAcquired : ScriptableObject
    {
        [SerializeField] bool dash;
        [SerializeField] bool ray;

        public bool DashAcquired
        {
            get => dash;
            set => dash = value;
        }

        public bool RayAcquired
        {
            get => ray;
            set => ray = value;
        }

        public bool Acquired(Ability ability)
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

        public void Acquire(Ability ability)
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
