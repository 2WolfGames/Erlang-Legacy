using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(menuName = "Player/HP")]
    public class PlayerHealth : ScriptableObject
    {
        public int HP;
        public int MaxHP;

    }
}
