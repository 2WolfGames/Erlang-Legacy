using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(menuName = "Player/HP")]
    public class Health : ScriptableObject
    {
        public int HP;
        public int MaxHP;
    }
}
