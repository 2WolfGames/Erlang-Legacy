using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.IA.Bahavior.SharedVariable
{
    [System.Serializable]
    public class SharedSprite : SharedVariable<Sprite>
    {
        public static implicit operator SharedSprite(Sprite value) { return new SharedSprite { Value = value }; }
    }
}
